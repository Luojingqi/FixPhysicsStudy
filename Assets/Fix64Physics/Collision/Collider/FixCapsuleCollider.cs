using Fix64Physics.Data;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fix64Physics.Collision
{
    /// <summary>
    /// 胶囊
    /// </summary>
    public class FixCapsuleCollider : FixCollider
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
            Fix64 ixz = new Fix64(1, 4) * (h.Sq() + Fix64.ahalf * r.Sq());
            Fix64 iy = Fix64.ahalf * r.Sq();
            inertia = new FixMatrix3x3(
                ixz, Fix64.zero, Fix64.zero,
                Fix64.zero, iy, Fix64.zero,
                Fix64.zero, Fix64.zero, ixz);
        }


        public override void CalculateVolume()
        {
            volume = new Fix64(4, 3) * Fix64.PI * Fix64.Pow(r, 3) + Fix64.PI * r.Sq() * h;
        }

        public override bool Raycast(FixRay ray, out FixRaycastHit hit)
        {
            throw new NotImplementedException();
        }
    }
}
