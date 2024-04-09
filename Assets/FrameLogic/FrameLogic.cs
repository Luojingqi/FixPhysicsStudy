using Fix64Physics;
using Fix64Physics.Data;
using FrameMessage;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using TransformReplace;
using UnityEngine;

namespace FrameLogicFramework
{
    /// <summary>
    /// 帧同步主逻辑
    /// </summary>
    public class FrameLogic
    {
        /// <summary>
        /// 定点数物理系统
        /// </summary>
        public FixPhysicsSystem physicsSystem = new FixPhysicsSystem();
        public AllGameFrameData AllData;
        /// <summary>
        /// 渲染帧时间总和
        /// </summary>
        public float renderingTimeSum { get; private set; } = 0;
        /// <summary>
        /// 设定的逻辑帧更新间隔
        /// </summary>
        public float frameTimeLen { get; private set; }
        public Fix64 fixFrameTimeLen { get; set; }
        /// <summary>
        /// 代理对象
        /// </summary>
        public List<IFrameLogicAgent> agentList = new List<IFrameLogicAgent>();

        public List<IOnlyRender> onlyRenders = new List<IOnlyRender>();

        public event Action OnUpdateLogic;
        public event Action OnUpdateRender;

#if _CLIENTLOGIC_
        /// <summary>
        /// 客户端帧数据发送
        /// </summary>
        public Action FrameDataSend;

        /// <summary>
        /// 延迟触发集合
        /// </summary>
        private List<DelayAction> DelayActionList = new List<DelayAction>();

        public void AddDelay(Fix64 time, Action action)
        {
            var obj = DelayActionPool.TakeOut();
            obj.time = time;
            obj.action = action;
            DelayActionList.Add(obj);
        }
        private class DelayAction
        {
            public Fix64 time;
            public Action action;
        }
        private ObjectPool<DelayAction> DelayActionPool = new ObjectPool<DelayAction>(50, true);
#else
        /// <summary>
        /// 服务端帧数据发送
        /// </summary>
        public Action<FrameDataList> FrameDataSend;
        private FrameDataList newFrameDataDic = new FrameDataList();
#endif

        public FrameLogic()
        {

        }

        public void Init(Fix64 fixFrameTimeLen)
        {
            this.fixFrameTimeLen = fixFrameTimeLen;
            frameTimeLen = (float)fixFrameTimeLen;
            renderingTimeSum = 0;
            agentList.Clear();
        }
        public bool pause = false;
        /// <summary>
        /// 帧同步逻辑更新
        /// </summary>
        /// <param name="dataList">当前一帧所有玩家的操作</param>
        public void UpdateLogic()
        {
            if (!AllData.IsRun) return;
            float deltaTime = 0;
#if _CLIENTLOGIC_
            deltaTime = UnityEngine.Time.deltaTime;
#else
            deltaTime = frameTimeLen;
#endif
            renderingTimeSum += deltaTime;
            //渲染帧大于逻辑帧,开始更新逻辑
            while (renderingTimeSum > frameTimeLen)
            {
                bool ReceivedAllData = true;
                #region 分别检测是否收到当前帧数据
#if _CLIENTLOGIC_
                // Debug.Log(AllData.FrameDataDic[AllData.PlayerID].Count + "|" + AllData.NowGameLogicFrameIndex);
                //客户端检测是否收到数据
                if (AllData.FrameDataDic[AllData.PlayerID].Count <= AllData.NowGameLogicFrameIndex)
                    ReceivedAllData = false;
#else
                //服务端检测是否收到每个客户端数据
                newFrameDataDic.Value.Clear();
                newFrameDataDic.FrameIndex = AllData.NowGameLogicFrameIndex;
                foreach (var ID_FrameDataList in AllData.FrameDataDic)
                {
                    if (ID_FrameDataList.Value.Count > AllData.NowGameLogicFrameIndex)
                    {
                        newFrameDataDic.Value.Add(ID_FrameDataList.Value[AllData.NowGameLogicFrameIndex]);
                    }
                    else
                    {
                        ReceivedAllData = false;
                        break;
                    }
                }
#endif
                #endregion

                if (ReceivedAllData)
                {
#if _CLIENTLOGIC_
                    //客户端执行游戏逻辑相关代码
                    foreach (var agent in agentList)
                    {
                        agent.SaveLogicTransform0();
                        FrameData frameData = AllData.FrameDataDic[agent.PlayerID][AllData.NowGameLogicFrameIndex];
                        if (frameData.AgentData.TryGetValue((int)agent.AgentType, out Operate operate))
                            if (operate.Value.TryGetValue(agent.AgentID, out ByteString bytes))
                                agent.GameLogicDataUpdata(bytes);
                    }
                    //延迟触发事件检查
                    for (int i = 0; i < DelayActionList.Count; i++)
                    {
                        DelayActionList[i].time -= fixFrameTimeLen;
                        if (DelayActionList[i].time <= Fix64.zero)
                        {
                            DelayActionList[i].action();
                            DelayActionPool.PutIn(DelayActionList[i]);
                            DelayActionList.RemoveAt(i);
                        }
                    }
                    OnUpdateLogic?.Invoke();

                    if (pause == false)
                        //每逻辑帧更新两次物理
                        for (int i = 0; i < 2; i++)
                            physicsSystem.PhysicsUpdate();


                    foreach (var agent in agentList)
                    {
                        agent.SaveLogicTransform1();
                    }
#endif

                    //逻辑帧索引++
                    AllData.NowGameLogicFrameIndex++;
                    //重置渲染帧
                    renderingTimeSum = 0;

                    if (!AllData.IsPlayBack)
                    {
#if _CLIENTLOGIC_
                        FrameDataSend?.Invoke();
#else
                        FrameDataSend?.Invoke(newFrameDataDic);
#endif
                    }
                }
                else break;
            }

            float timeTween = renderingTimeSum;
            float frameTween = renderingTimeSum / frameTimeLen;
            if (frameTween <= 1)
            {
                foreach (var agent in agentList)
                    agent.GameRenderPosition(timeTween, frameTween, deltaTime);
                foreach (var agent in onlyRenders)
                    agent.GameRenderPosition(timeTween, frameTween, deltaTime);
                OnUpdateRender?.Invoke();
            }
        }
    }
}
