using Fix64Physics.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace TransformReplace
{
    /// <summary>
    /// 用于替代unity自带的Transform
    /// </summary>
    public class FixTransformMono : SerializedMonoBehaviour, IFixTransform
    {

#if _CLIENTLOGIC_
        [OdinSerialize]
        //[HideInInspector]
#endif
        private FixVector3 position;
#if _CLIENTLOGIC_
        private void OnChangedLocalPosition()
        {
            if (!Application.isPlaying) transform.localPosition = (Vector3)localPosition;
            LocalPosition = localPosition;
        }
        [OdinSerialize]
        [LabelWidth(50)]
        [OnValueChanged("OnChangedLocalPosition")]
#endif
        private FixVector3 localPosition;
#if _CLIENTLOGIC_
        [OdinSerialize]
        // [HideInInspector]
#endif
        private FixQuaternion rotation = FixQuaternion.identity;
#if _CLIENTLOGIC_
        private void OnChangedLocalRotation()
        {
#if _CLIENTLOGIC_
            if (!Application.isPlaying) transform.localRotation = (Quaternion)localRotation;
#endif
            LocalRotation = localRotation;
        }
        [OdinSerialize]
        [LabelWidth(50)]
        [OnValueChanged("OnChangedLocalRotation")]
#endif
        private FixQuaternion localRotation = FixQuaternion.identity;
#if _CLIENTLOGIC_
        private void OnChangedLocalScale()
        {
#if _CLIENTLOGIC_
            if (!Application.isPlaying) transform.localScale = (Vector3)localScale;
#endif
            LocalScale = localScale;
        }
        [OdinSerialize]
        [LabelWidth(50)]
        [OnValueChanged("OnChangedLocalScale")]
#endif
        private FixVector3 localScale = FixVector3.one;
#if _CLIENTLOGIC_
        [OdinSerialize]
        [HideInInspector]
#endif
        private FixVector3 lossyScale = FixVector3.one;
#if _CLIENTLOGIC_
        [LabelWidth(50)]
        [Button("设置父物体")]
        private void OnChangedParent(IFixTransform parent)
        {
            Parent = parent;
        }

        [OdinSerialize]
        [LabelWidth(50)]
        [ReadOnly]
#endif
        private IFixTransform parent;
#if _CLIENTLOGIC_
        [OdinSerialize]
        //[HideInInspector]
#endif
        private List<IFixTransform> childList = new List<IFixTransform>();

        [OdinSerialize]
        public FixVector3 Position
        {
            get => position;
            set
            {
                position = value;
                if (parent != null)
                {
                    localPosition = parent.FixInverseTransformPoint(position);
                }
                else
                {
                    localPosition = position;
                }
                UpdateChildsPosition();
            }
        }
        public FixVector3 LocalPosition
        {
            get => localPosition;
            set
            {
                localPosition = value;
                if (parent != null)
                {
                    position = parent.FixTransformPoint(localPosition);
                }
                else
                {
                    position = localPosition;
                }
                UpdateChildsPosition();
            }
        }
        private void UpdateChildsPosition()
        {
            for (int i = 0; i < childList.Count; i++)
            {
                childList[i].LocalPosition = childList[i].LocalPosition;
            }
        }
        public FixQuaternion Rotation
        {
            get => rotation;
            set
            {
                rotation = value.Normalize();
                if (parent != null)
                {
                    localRotation = (parent.Rotation.Inverse() * rotation).Normalize();
                }
                else
                {
                    localRotation = rotation;
                }
                UpdateChildsRotation();
                UpdateChildsPosition();
            }
        }
        public FixQuaternion LocalRotation
        {
            get => localRotation;
            set
            {
                localRotation = value.Normalize();
                if (parent != null)
                {
                    rotation = (parent.Rotation * localRotation).Normalize();
                }
                else
                {
                    rotation = localRotation;
                }
                UpdateChildsRotation();
                UpdateChildsPosition();
            }
        }
        private void UpdateChildsRotation()
        {
            foreach (var child in childList)
            {
                child.LocalRotation = child.LocalRotation;
            }
        }
        public FixVector3 LocalScale
        {
            get => localScale;
            set
            {
                localScale = value;
                if (parent != null)
                {
                    lossyScale = parent.LossyScale * localScale;
                }
                else
                {
                    lossyScale = localScale;
                }
                UpdateChildsScale();
            }
        }
        public FixVector3 LossyScale { get { return lossyScale; } }
        private void UpdateChildsScale()
        {
            foreach (var child in childList)
            {
                child.LocalScale = child.LocalScale;
                //child.Position += Rotation * (child.LocalPosition * child.LocalScale);
            }
        }

        public IFixTransform Parent
        {
            get => parent;
            set
            {
                if ((object)value == this)
                {
                    Debug.LogError("请勿将父物体设置为自己");
                    return;
                }
                IFixTransform oldParent = parent;
                IFixTransform newParent = value;
                if (oldParent != null) oldParent.ChildList.Remove(this);
                if (newParent != null)
                {
                    transform.parent = newParent.transform;
                    // Debug.Log(transform.localRotation);
                    newParent.ChildList.Add(this);
                    localPosition = newParent.FixInverseTransformPoint(this.position, out FixQuaternion rInverse);
                    localRotation = (rInverse * rotation).Normalize();
                    //localScale = (lossyScale / newParent.LossyScale) * localRotation.Inverse();
                }
                else
                {
                    transform.parent = null;
                    localPosition = position;
                    localRotation = rotation;
                    //localScale = lossyScale;
                }
                parent = newParent;
            }
        }
        public List<IFixTransform> ChildList { get => childList; }

        private void InitGetChild()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.TryGetComponent(out IFixTransform agent))
                {
                    ChildList.Add(agent);
                }
            }
        }
        private void InitGetParent()
        {
            if (transform.parent != null)
                if (transform.parent.TryGetComponent(out IFixTransform agent))
                {
                    Parent = agent;
                }
        }


        public virtual void Awake()
        {
            //InitGetParent();
            //InitGetChild();
        }
    }

    public static class FixTransformEx
    {
        /// <summary>
        /// 将世界坐标系的点转换到agent的局部坐标系
        /// </summary>
        public static FixVector3 FixInverseTransformPoint(this IFixTransform agent, FixVector3 pos)
        {
            pos -= agent.Position;
            pos *= agent.Rotation.Inverse();
            //  pos /= agent.LossyScale;
            return pos;
        }
        public static FixVector3 FixInverseTransformPoint(this IFixTransform agent, FixVector3 pos, out FixQuaternion rInverse)
        {
            rInverse = agent.Rotation.Inverse();
            pos -= agent.Position;
            pos *= rInverse;
            //  pos /= agent.LossyScale;
            return pos;
        }
        /// <summary>
        /// 将agent局部坐标系的点转换到世界坐标系
        /// </summary>
        public static FixVector3 FixTransformPoint(this IFixTransform agent, FixVector3 pos)
        {
            // pos *= agent.LossyScale;
            pos *= agent.Rotation;
            pos += agent.Position;
            return pos;
        }

        /// <summary>
        /// 同时更新逻辑位置和渲染位置
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="pos"></param>
        public static void SetPosition(this IFixTransform agent, FixVector3 pos)
        {
            agent.Position = pos;
            agent.transform.position = (Vector3)pos;
        }
        /// <summary>
        /// 同时更新逻辑位置和渲染位置
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="pos"></param>
        public static void SetLocalPosition(this IFixTransform agent, FixVector3 pos)
        {
            agent.LocalPosition = pos;
            agent.transform.localPosition = (Vector3)pos;
        }
        /// <summary>
        /// 同时更新逻辑位置和渲染位置
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="pos"></param>
        public static void SetRotation(this IFixTransform agent, FixQuaternion fixQuaternion)
        {
            agent.Rotation = fixQuaternion;
            agent.transform.rotation = (Quaternion)fixQuaternion;
        }

        public static void LookAtWorld(this IFixTransform agent, FixVector3 WorldPos, bool lockXZ = false)
        {
            FixVector3 f = FixVector3.forward;
            FixVector3 l = WorldPos - agent.Position;
            if (lockXZ) l.y = Fix64.zero; 
            FixVector3 fn = f.Normalize();
            FixVector3 ln = l.Normalize();

            FixVector3 halfn = (fn + ln).Normalize();

            Fix64 cosahalf = FixVector3.Dot(fn, halfn);
            FixVector3 sinahalf = FixVector3.Cross(fn, halfn);

            FixQuaternion q = new FixQuaternion(cosahalf, sinahalf);

            agent.LocalRotation = q; //* agent.LocalRotation;
        }
        public static void LookAtLocal(this IFixTransform agent, FixVector3 LocalPos, bool lockXZ = false)
        {
            FixVector3 f = FixVector3.forward;
            FixVector3 l = LocalPos - agent.LocalPosition;
            if (lockXZ) l.y = Fix64.zero;
            FixVector3 fn = f.Normalize();
            FixVector3 ln = l.Normalize();

            FixVector3 halfn = (fn + ln).Normalize();

            Fix64 cosahalf = FixVector3.Dot(fn, halfn);
            FixVector3 sinahalf = FixVector3.Cross(fn, halfn);

            FixQuaternion q = new FixQuaternion(cosahalf, sinahalf);

            agent.LocalRotation = q * agent.LocalRotation;
        }
    }
}
