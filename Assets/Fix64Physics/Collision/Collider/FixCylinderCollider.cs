using Fix64Physics.Data;
using Sirenix.Serialization;
using System;
using System.Drawing;

namespace Fix64Physics.Collision
{
    /// <summary>
    /// 圆柱
    /// </summary>
    public class FixCylinderCollider : FixCollider
    {
#if _CLIENTLOGIC_
        [OdinSerialize]
#endif
        private Fix64 r = Fix64.ahalf;
        public Fix64 R { get { return r; } set { r = value; } }
#if _CLIENTLOGIC_
        [OdinSerialize]
#endif
        private Fix64 h = new Fix64(2);
        public Fix64 H { get { return h; } set { h = value; } }

        public override void CalculateAABB()
        {
            Fix64 temp = h * Fix64.ahalf;
            _aabb.min = new FixVector3(-r, -temp, -r);
            _aabb.max = new FixVector3(r, temp, r);
        }
        public override void CalculateInertia()
        {
            Fix64 ixy = new Fix64(1, 12) * (3 * r.Sq() + h.Sq());
            Fix64 iz = Fix64.ahalf * r.Sq();
            inertia = new FixMatrix3x3(
                ixy, Fix64.zero, Fix64.zero,
                Fix64.zero, ixy, Fix64.zero,
                Fix64.zero, Fix64.zero, iz);
        }

        public override void CalculateVolume()
        {
            volume = Fix64.PI * r.Sq() * h;
        }

        public override bool Raycast(FixRay ray, out FixRaycastHit hit)
        {
            throw new NotImplementedException();
        }
    }
}
