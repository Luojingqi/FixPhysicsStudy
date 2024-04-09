using Fix64Physics.Data;
using Sirenix.Serialization;

namespace Fix64Physics.Collision
{
    public class FixPlaneCollider : FixCollider
    {
#if _CLIENTLOGIC_
        [OdinSerialize]
#endif
        private FixVector2 size = FixVector2.one * new Fix64(10);
        public FixVector2 Size { get { return size; } set { size = value; } }

        public override void CalculateAABB()
        {
            FixVector3 temp = new FixVector3(size.x, Fix64.zero, size.y) * Fix64.ahalf;
            _aabb.min = -temp;
            _aabb.max = temp;
        }

        public override void CalculateInertia()
        {

        }

        public override void CalculateVolume()
        {

        }

        public override bool Raycast(FixRay ray, out FixRaycastHit hit)
        {
            throw new System.NotImplementedException();
        }
    }
}
