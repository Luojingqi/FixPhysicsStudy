using Fix64Physics.Data;
using TransformReplace;

namespace Fix64Physics.Collision
{
    public struct FixRaycastHit
    {
        public IFixTransform fixTransform;
        public FixCollider collider;
        /// <summary>
        /// 碰撞点
        /// </summary>
        public FixVector3 point;
        /// <summary>
        /// 碰撞法线
        /// </summary>
        public FixVector3 normal;
        /// <summary>
        /// 距离
        /// </summary>
        public Fix64 distance;
    }
}
