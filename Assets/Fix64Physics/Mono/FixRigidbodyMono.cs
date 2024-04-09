using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TransformReplace;
using UnityEngine;

namespace Fix64Physics.Mono
{
    public class FixRigidbodyMono : SerializedMonoBehaviour, IOnlyRender
    {

        public FixRigidbody _fixRigidbody = new FixRigidbody();
        public FixRigidbody fixRigidbody { get { return _fixRigidbody; } }
        private IFixTransform fixTransform;
        public IFixTransform FixTransform => fixTransform;

        public void Start()
        {
            Init();
        }
        public void OnEnable()
        {
            FixPhysicsSystem.Inst.Add(this);
        }
        public void OnDisable()
        {
            FixPhysicsSystem.Inst.Remove(this);
        }
        public void Init()
        {
            //Debug.Log(transform.name);
            fixTransform = transform.GetComponent<IFixTransform>();
            _fixRigidbody.FixTransform = fixTransform;
            foreach (var mono in transform.GetComponentsInChildren<FixColliderMono>())
            {
                if (mono.FixCollider.IsTrigger) continue;
                fixRigidbody.colliderList.Add(mono.FixCollider);
                mono.FixCollider.fixRigidbody = fixRigidbody;
            }
            fixRigidbody.Init();
            position0 = (Vector3)fixTransform.Position;
            rotation0 = (Quaternion)fixTransform.Rotation;
            position1 = (Vector3)fixTransform.Position;
            rotation1 = (Quaternion)fixTransform.Rotation;
        }
        #region 记录转换后的上一帧数据
        [OdinSerialize]
        [HideInInspector]
        private Vector3 position0;
        [OdinSerialize]
        [HideInInspector]
        private Quaternion rotation0;
        #endregion
        #region 记录转换后的当前帧数据
        [OdinSerialize]
        [HideInInspector]
        private Vector3 position1;
        [OdinSerialize]
        [HideInInspector]
        private Quaternion rotation1;
        #endregion
        public void GameRenderPosition(float timeTween, float frameTween, float deltaTime)
        {
            transform.position = Vector3.Lerp(position0, position1, frameTween);
            transform.rotation = Quaternion.Lerp(rotation0, rotation1, frameTween);
        }


        /// <summary>
        /// 物理更新，在固定间隔触发
        /// </summary>
        public void PhysicsUpdate()
        {
            position0 = (Vector3)fixTransform.Position;
            rotation0 = (Quaternion)fixTransform.Rotation;
            fixRigidbody.FixedUpdate();
            position1 = (Vector3)fixTransform.Position;
            rotation1 = (Quaternion)fixTransform.Rotation;
        }
    }
}
