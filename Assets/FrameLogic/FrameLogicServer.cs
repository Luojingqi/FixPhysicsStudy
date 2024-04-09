using Fix64Physics.Data;
using FrameLogicFramework;
using FrameMessage;
using System;
using System.Collections.Generic;
using TransformReplace;


/// <summary>
/// 从服务端拷贝而来，用于在客户端内也可以跑服务器
/// </summary>
public class FrameLogicServer
{

    public AllGameFrameData AllData { get; set; }
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


    /// <summary>
    /// 服务端帧数据发送
    /// </summary>
    public Action<FrameDataList> FrameDataSend;
    private FrameDataList newFrameDataDic = new FrameDataList();




    public void Init(Fix64 fixFrameTimeLen, AllGameFrameData allData)
    {
        this.fixFrameTimeLen = fixFrameTimeLen;
        frameTimeLen = (float)fixFrameTimeLen;
        this.AllData = allData;
        renderingTimeSum = 0;
        agentList.Clear();
    }
    /// <summary>
    /// 帧同步逻辑更新
    /// </summary>
    /// <param name="dataList">当前一帧所有玩家的操作</param>
    public void UpdateLogic()
    {
        if (!AllData.IsRun) return;
        float deltaTime = 0;

        deltaTime = UnityEngine.Time.deltaTime;

        renderingTimeSum += deltaTime;
        //渲染帧大于逻辑帧,开始更新逻辑
        while (renderingTimeSum > frameTimeLen)
        {
            bool ReceivedAllData = true;
            #region 分别检测是否收到当前帧数据
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
            #endregion

            if (ReceivedAllData)
            {

                //逻辑帧索引++
                AllData.NowGameLogicFrameIndex++;
                //重置渲染帧
                renderingTimeSum = 0;

                if (!AllData.IsPlayBack)
                {

                    FrameDataSend?.Invoke(newFrameDataDic);

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
        }
    }
}

