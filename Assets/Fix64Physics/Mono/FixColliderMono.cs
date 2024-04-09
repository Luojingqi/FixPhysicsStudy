using Fix64Physics.Collision;
using Sirenix.OdinInspector;
using TransformReplace;

namespace Fix64Physics.Mono
{
    public class FixColliderMono : SerializedMonoBehaviour
    {
        public IFixTransform FixTransform => FixCollider.fixTransform;
        public FixCollider FixCollider;
        public bool auto;
        public void Awake()
        {
            Init();

        }
        public void Init()
        {
            FixCollider.fixTransform = transform.GetComponent<IFixTransform>();
            FixCollider.CalculateAABB();
        }
        private void OnEnable()
        {
            if (auto) Open();
        }
        private void OnDisable()
        {
            if (auto) Close();
        }
        [Button]
        public void Open()
        {
            FixPhysicsSystem.Inst.Add(FixCollider);
        }

        public void Close()
        {
            FixPhysicsSystem.Inst.Remove(FixCollider);
            if (FixCollider.fixRigidbody != null)
            {
                FixCollider.fixRigidbody.colliderList.Remove(FixCollider);
                FixCollider.fixRigidbody = null;
            }
        }
    }
}
