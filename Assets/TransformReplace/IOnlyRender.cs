using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransformReplace
{
    /// <summary>
    /// 需要实现逻辑与渲染分离的实现此接口
    /// 仅渲染
    /// </summary>
    public interface IOnlyRender
    {
        public IFixTransform FixTransform { get; }
        /// <summary>
        /// 渲染帧触发，根据补间间隔计算补间动画
        /// </summary>
        /// <param name="timeTween">相差时间</param>
        /// <param name="frameTween">相差帧</param>
        void GameRenderPosition(float timeTween, float frameTween, float deltaTime);
    }
}
