using Google.Protobuf;
using TransformReplace;

namespace FrameLogicFramework
{
    /// <summary>
    /// 帧同步代理对象接口
    /// 实现逻辑与渲染分离都需要此接口
    /// </summary>
    public interface IFrameLogicAgent
    {
        /// <summary>
        /// 游戏逻辑帧触发，更新逻辑位置
        /// </summary>
        public void GameLogicDataUpdata(ByteString data);
        /// <summary>
        /// 渲染帧触发，根据补间间隔计算补间动画
        /// </summary>
        /// <param name="timeTween">相差时间</param>
        /// <param name="frameTween">相差帧</param>
        public void GameRenderPosition(float timeTween, float frameTween, float deltaTime);
        /// <summary>
        /// 游戏对象代理的ID
        /// </summary>
        public int AgentID { get; set; }
        /// <summary>
        /// 代理对象的类型
        /// </summary>
        public AgentType AgentType { get; set; }
        /// <summary>
        /// 游戏对象所属的玩家id
        /// </summary>
        public string PlayerID { get; set; }

        public IFixTransform FixTransform { get; }

        public void Init();

        public void SaveLogicTransform0();
        public void SaveLogicTransform1();
    }
    /// <summary>
    /// 代理对象类型
    /// </summary>
    public enum AgentType
    {
        None,
        GoapCom,
        AI,
        Ship,
    }
}
