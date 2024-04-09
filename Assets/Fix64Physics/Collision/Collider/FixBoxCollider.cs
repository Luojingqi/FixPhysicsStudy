using Fix64Physics.Data;
using Sirenix.Serialization;

namespace Fix64Physics.Collision
{
    public class FixBoxCollider : FixCollider
    {
#if _CLIENTLOGIC_
        [OdinSerialize]
#endif
        private FixVector3 size = FixVector3.one;
        public FixVector3 Size { get { return size; } set { size = value; } }

        public override void CalculateAABB()
        {
            FixVector3 temp = size * Fix64.ahalf;
            _aabb.min = -temp;
            _aabb.max = temp;
        }

        public override void CalculateInertia()
        {
            Fix64 fix = new Fix64(1, 12);
            Fix64 ix = fix * (size.y.Sq() + size.z.Sq());
            Fix64 iy = fix * (size.x.Sq() + size.z.Sq());
            Fix64 iz = fix * (size.x.Sq() + size.y.Sq());
            inertia = new FixMatrix3x3(
                ix, Fix64.zero, Fix64.zero,
                Fix64.zero, iy, Fix64.zero,
                Fix64.zero, Fix64.zero, iz);
        }

        public override void CalculateVolume()
        {
            volume = size.x * size.y * size.z;
        }

        public override bool Raycast(FixRay ray, out FixRaycastHit hit)
        {
            throw new System.NotImplementedException();
        }
    }
}
