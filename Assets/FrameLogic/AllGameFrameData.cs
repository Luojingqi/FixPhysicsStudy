using FrameMessage;
using System.Collections.Generic;


namespace FrameLogicFramework
{
    public class AllGameFrameData
    {
        /// <summary>
        /// 本机玩家ID
        /// </summary>
        public string PlayerID;
        /// <summary>
        /// 是否运行中
        /// </summary>
        public bool IsRun = false;
        /// <summary>
        /// 是否是回放模式
        /// </summary>
        public bool IsPlayBack = false;
        /// <summary>
        /// 游戏的当前逻辑帧索引
        /// </summary>
        public int NowGameLogicFrameIndex = 0;
        //随机数对象
        //public static SRandom g_srand = new SRandom(1000);
        /// <summary>
        /// 所有帧数据，服务器存储结构
        /// </summary>
        public Dictionary<string, List<FrameData>> FrameDataDic = new Dictionary<string, List<FrameData>>();
        /// <summary>
        /// 玩家的索引，因为所有玩家在同一个场景中，索引位每增加1，z轴坐标增加500,实现在MapBlock中
        /// </summary>
        public Dictionary<string, int> PlayerIndex = new Dictionary<string, int>();
        /// <summary>
        /// 所有玩家游戏开始的信息
        /// </summary>
        public PlayerGameDataList PlayerStartDataList;
    }
}
