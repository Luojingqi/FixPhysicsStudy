

using Fix64Physics.Data;
using System.Collections.Generic;

namespace Fix64Physics
{
    /// <summary>
    /// 物理全局字段
    /// </summary>
    public static class FixPhysicsGlobal
    {
        /// <summary>
        /// 固定物理时间
        /// </summary>
        public static Fix64 fixedDeltaTime = new Fix64(1, 40);
        public static Fix64 fixedDeltaTimeSq = fixedDeltaTime.Sq();
        //{9.806884765625} ==  9807/1000 ==  40169L
        public static Fix64 g = new Fix64(40169L);
        public static FixVector3 G = new FixVector3(Fix64.zero, -g, Fix64.zero);
        /// <summary>
        /// 默认线性阻尼
        /// </summary>
        public static Fix64 linearDrag = new Fix64(199, 200);
        /// <summary>
        /// 默认旋转阻尼
        /// </summary>
        public static Fix64 rotateDrag = new Fix64(199, 200);
        /// <summary>
        /// 默认弹性系数
        /// </summary>
        public static Fix64 elasticCoefficient = new Fix64(1, 4);
        /// <summary>
        /// 默认摩擦系数
        /// </summary>
        public static Fix64 frictionalCoefficient = new Fix64(1, 3);
        /// <summary>
        /// 默认最大角速度
        /// </summary>
        public static Fix64 maxAngularVelocity = new Fix64(7);
        /// <summary>
        /// 睡眠速度（小于进入睡眠状态）
        /// </summary>
        public static Fix64 sleepVelocityThreshold = new Fix64(15, 100).Sq();
        /// <summary>
        /// 睡眠角速度（小于进入睡眠状态）
        /// </summary>
        public static Fix64 sleepAngularVelocityThreshold = new Fix64(2, 10).Sq();
        /// <summary>
        /// 线性投影更新阈值
        /// </summary>
        public static Fix64 linearMoveThreshold = new Fix64(100L).Sq();

    }
}