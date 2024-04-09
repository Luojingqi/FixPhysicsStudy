using Fix64Physics.Data;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Fix64Physics.Collision
{
    /// <summary>
    /// 球
    /// </summary>
    public class FixSphereCollider : FixCollider
    {
#if _CLIENTLOGIC_
        [OdinSerialize]
#endif
        private Fix64 r = Fix64.ahalf;
        public Fix64 R { get { return r; } set { r = value; } }

        public override void CalculateAABB()
        {
            _aabb.min = new FixVector3(-r, -r, -r);
            _aabb.max = new FixVector3(r, r, r);
        }

        public override void CalculateInertia()
        {
            Fix64 i = new Fix64(2, 5) * r.Sq();
            inertia = new FixMatrix3x3(
                i, Fix64.zero, Fix64.zero,
                Fix64.zero, i, Fix64.zero,
                Fix64.zero, Fix64.zero, i);
        }

        public override void CalculateVolume()
        {
            volume = new Fix64(4, 3) * Fix64.PI * Fix64.Pow(r, 3);
        }

        public override bool Raycast(FixRay ray, out FixRaycastHit hit)
        {
            hit = new FixRaycastHit();
            FixVector3 D = Position - ray.origin;
            Fix64 dot = FixVector3.Dot(D, ray.direction);

            // 计算射线与球面的交点
            Fix64 dSq = D.MagnitudeSq();
            Fix64 rSq = r * r;
            if (dSq > rSq)
            {
                if (dot == Fix64.zero)
                {
                    // 球心方向和射线垂直且射线起点在球外
                    return false;
                }
                // 如果球在射线的反方向上，不可能相交
                if (dot < Fix64.zero)
                {
                    return false;
                }
            }

            //计算圆心距离射线最近的点的长度的平方
            Fix64 mSq = D.MagnitudeSq() - dot.Sq();
            //射线和球不相交
            if (mSq > rSq) return false;

            Fix64 q = (rSq - mSq).Sqrt();

            Fix64 l = dot - q;

            // 计算交点位置
            FixVector3 p = ray.origin + ray.direction * l;

            // 填充 hit 对象
            hit.point = p;
            hit.distance = l;
            hit.normal = (p - Position).Normalize();
            hit.collider = this;
            hit.fixTransform = fixTransform;

            return true;
        }
    }
}
