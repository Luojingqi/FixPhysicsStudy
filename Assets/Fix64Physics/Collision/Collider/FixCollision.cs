using Fix64Physics.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fix64Physics.Collision
{
    /// <summary>
    /// 碰撞数据
    /// </summary>
    public struct FixCollision
    {
        public FixVector3 position;
        public FixVector3 normal;
        public Fix64 depth;

        public override string ToString()
        {
            return $"position  {position}\nnormal  {normal}  depth{depth}";
        }
    }

    public struct FixCollisionData
    {
        public FixVector3 position;
        public FixVector3 normal;
        public Fix64 depth;
        public FixCollider myCollider;
        public FixCollider otherCollider;

        public FixCollisionData(FixCollision collision, FixCollider my, FixCollider other)
        {
            myCollider = my;
            otherCollider = other;
            position = collision.position;
            normal = collision.normal;
            depth = collision.depth;
        }
        public override string ToString()
        {
            return $"position  {position}\nnormal  {normal}  depth{depth}";
        }
    }
}
