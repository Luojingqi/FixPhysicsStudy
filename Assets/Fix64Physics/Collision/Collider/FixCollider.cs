using Fix64Physics.Data;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using TransformReplace;
using UnityEngine;

namespace Fix64Physics.Collision
{
    public abstract class FixCollider
    {
        public HashSet<int> RayLayer = new HashSet<int>() { 0 };
        /// <summary>
        /// 触发器
        /// </summary>
        public bool IsTrigger = false;
#if _CLIENTLOGIC_
        [HideInInspector]
        [OdinSerialize]
#endif
        public FixRigidbody fixRigidbody;
#if _CLIENTLOGIC_
        [HideInInspector]
        [OdinSerialize]
#endif
        public IFixTransform fixTransform;
#if _CLIENTLOGIC_
        [OdinSerialize]
        [HideInInspector]
#endif
        protected Fix64 volume;
#if _CLIENTLOGIC_
        [OdinSerialize]
#endif
        protected FixVector3 centerOffset;

        protected AABB _aabb;
        /// <summary>
        ///  转动惯量矩阵
        /// </summary>
        protected FixMatrix3x3 inertia;

        public FixVector3 CenterOffset { get => centerOffset; set => centerOffset = value; }
        public FixVector3 Position { get { return fixTransform.Position + centerOffset * fixTransform.Rotation; } }
        public AABB aabb { get { return (_aabb + CenterOffset) * fixTransform.Rotation + fixTransform.Position; } }
        public FixMatrix3x3 Inertia => inertia;
        public Fix64 Volume { get => volume; }

        public event Action<FixCollisionData> OnCollisionEnter;
        public event Action<FixCollisionData> OnCollisionStay;
        public event Action<FixCollisionData> OnCollisionExit;

        public event Action<FixCollisionData> OnTriggerEnter;
        public event Action<FixCollisionData> OnTriggerStay;
        public event Action<FixCollisionData> OnTriggerExit;

        public void OnCollisionEnterInvoke(FixCollisionData data)
        {
            OnCollisionEnter?.Invoke(data);
        }
        public void OnCollisionStayInvoke(FixCollisionData data)
        {
            OnCollisionStay?.Invoke(data);
        }
        public void OnCollisionExitInvoke(FixCollisionData data)
        {
            OnCollisionExit?.Invoke(data);
        }
        public void OnTriggerEnterInvoke(FixCollisionData data)
        {
            OnTriggerEnter?.Invoke(data);
        }
        public void OnTriggerStayInvoke(FixCollisionData data)
        {
            OnTriggerStay?.Invoke(data);
        }
        public void OnTriggerExitInvoke(FixCollisionData data)
        {
            OnTriggerExit?.Invoke(data);
        }

        /// <summary>
        /// 计算转动惯量矩阵
        /// </summary>
        public abstract void CalculateInertia();
        /// <summary>
        /// 计算体积
        /// </summary>
        public abstract void CalculateVolume();
        /// <summary>
        /// 计算aabb包围盒
        /// </summary>
        public abstract void CalculateAABB();
        /// <summary>
        /// 射线检测
        /// </summary>
        public abstract bool Raycast(FixRay ray, out FixRaycastHit hit);
    }
}
