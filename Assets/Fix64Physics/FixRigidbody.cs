using Fix64Physics.Collision;
using Fix64Physics.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using TransformReplace;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Fix64Physics
{
    /// <summary>
    /// ����
    /// </summary>
    public class FixRigidbody
    {

        private IFixTransform fixTransform;
        public IFixTransform FixTransform { get { return fixTransform; } set { fixTransform = value; } }

        #region ��������
#if _CLIENTLOGIC_
        public Transform transform { get { return fixTransform.transform; } }
        [BoxGroup("��̬��")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(0)]
#endif
        public bool IsStatic;

#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup("����")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(1)]
#endif
        /// <summary>
        /// ����
        /// </summary>
        private Fix64 mass = Fix64.one;
        public Fix64 Mass { get { return mass; } set { mass = value; } }
#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup("�ٶ�")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(2)]
#endif
        /// <summary>
        /// �ٶ�
        /// </summary>
        private FixVector3 velocity;
        public FixVector3 Velocity { get { return velocity; } set { velocity = value; } }
#if _CLIENTLOGIC_
        [OdinSerialize]
        [HideInInspector]
#endif
        /// <summary>
        /// ���ٶ�
        /// </summary>
        private FixVector3 acceleration;
        public FixVector3 Acceleration { get { return acceleration; } }
#if _CLIENTLOGIC_
        [HideInInspector]
        [OdinSerialize]
#endif
        /// <summary>
        /// ����
        /// </summary>
        private FixVector3 impulse;
        public FixVector3 Impulse { get { return impulse; } }
#if _CLIENTLOGIC_
        private void OnChangedUseGravity()
        {
            if (useGravity) netForce += FixPhysicsGlobal.G * Mass;
            else netForce -= FixPhysicsGlobal.G * Mass;
        }
        [OnValueChanged("OnChangedUseGravity")]
        [OdinSerialize]
        [BoxGroup("ʹ������")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(4)]
#endif
        private bool useGravity;
        public bool UseGravity
        {
            get { return useGravity; }
            set
            {
                if (value == useGravity) return;
                useGravity = value;
                if (useGravity) netForce += FixPhysicsGlobal.G * Mass;
                else netForce -= FixPhysicsGlobal.G * Mass;
            }
        }
#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup(@"����\����")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(6)]
#endif
        /// <summary>
        /// ����
        /// </summary>
        private Fix64 linearDrag = FixPhysicsGlobal.linearDrag;
        public Fix64 LinearDrag { get { return linearDrag; } }
#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup(@"����\��ת")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(7)]
#endif
        /// <summary>
        /// ����
        /// </summary>
        private Fix64 rotateDrag = FixPhysicsGlobal.rotateDrag;
        public Fix64 RotateDrag { get { return rotateDrag; } }

#if _CLIENTLOGIC_
        [HideInInspector]
        [OdinSerialize]
#endif
        /// <summary>
        /// ������
        /// </summary>
        private FixVector3 netForce;
        public FixVector3 NetForce { get { return netForce; } set { netForce = value; } }


#if _CLIENTLOGIC_
        [OdinSerialize]
        [HideInInspector]
#endif
        /// <summary>
        /// �Ǽ��ٶ�
        /// </summary>
        private FixVector3 angularAcceleration;
        public FixVector3 AngularAcceleration { get { return angularAcceleration; } }
#if _CLIENTLOGIC_
        [PropertyOrder(3)]
        [OdinSerialize]
        [BoxGroup("���ٶ�")]
        [HideLabel]
        [LabelWidth(50)]
#endif
        /// <summary>
        /// ���ٶ�
        /// </summary>
        private FixVector3 angularVelocity;
        public FixVector3 AngularVelocity { get { return angularVelocity; } }

#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup("�����ٶ�(����)")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(10)]
#endif
        public Fix64 maxAngularVelocity = FixPhysicsGlobal.maxAngularVelocity;
#if _CLIENTLOGIC_
        [HideInInspector]
        [OdinSerialize]
#endif
        private FixMatrix3x3 inertia;
        /// <summary>
        /// ת����������
        /// </summary>
        public FixMatrix3x3 Inertia { get { return inertia; } set { inertia = value; } }
#if _CLIENTLOGIC_
        [HideInInspector]
        [OdinSerialize]
#endif
        private FixVector3 netTorque;
        /// <summary>
        /// Ť��
        /// </summary>
        public FixVector3 NetTorque { get { return netTorque; } set { netTorque = value; } }

#if _CLIENTLOGIC_
        [HideInInspector]
        [OdinSerialize]
#endif
        /// <summary>
        /// �ǳ���
        /// </summary>
        private FixVector3 angularImpulse;
        public FixVector3 AngularImpulse { get { return angularImpulse; } }


#if _CLIENTLOGIC_
        [OdinSerialize]
        [HideInInspector]
#endif
        /// <summary>
        /// ��ת����
        /// </summary>
        private FixMatrix3x3 rotationLimitation = FixMatrix3x3.identity;

#if _CLIENTLOGIC_
        [Button("������ת����")]
#endif
        public void SetRotationLimitation(bool x = false, bool y = false, bool z = false)
        {
            rotationLimitation = FixMatrix3x3.identity;
            if (x) rotationLimitation.m00 = Fix64.zero;
            else rotationLimitation.m00 = Fix64.one;
            if (y) rotationLimitation.m11 = Fix64.zero;
            else rotationLimitation.m11 = Fix64.one;
            if (z) rotationLimitation.m22 = Fix64.zero;
            else rotationLimitation.m22 = Fix64.one;
        }
#if _CLIENTLOGIC_
        [OdinSerialize]
        [HideInInspector]
#endif
        /// <summary>
        /// �ƶ�����
        /// </summary>
        private FixMatrix3x3 positionLimitation = FixMatrix3x3.identity;
#if _CLIENTLOGIC_
        [Button("�����ƶ�����")]
#endif
        public void SetPositionLimitation(bool x = false, bool y = false, bool z = false)
        {
            positionLimitation = FixMatrix3x3.identity;
            if (x) positionLimitation.m00 = Fix64.zero;
            else positionLimitation.m00 = Fix64.one;
            if (y) positionLimitation.m11 = Fix64.zero;
            else positionLimitation.m11 = Fix64.one;
            if (z) positionLimitation.m22 = Fix64.zero;
            else positionLimitation.m22 = Fix64.one;
        }
#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup("����")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(5)]
#endif
        private FixVector3 centroid;
        /// <summary>
        /// ����(�ֲ�����)
        /// </summary>
        public FixVector3 Centrold { get { return centroid; } set { centroid = value; } }


#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup("����ϵ��")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(8)]
#endif
        /// <summary>
        /// ����ϵ��
        /// </summary>
        private Fix64 elasticCoefficient = FixPhysicsGlobal.elasticCoefficient;
        public Fix64 ElasticCoefficient { get { return elasticCoefficient; } }
#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup("Ħ��ϵ��")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(9)]
#endif
        private Fix64 frictionalCoefficient = FixPhysicsGlobal.frictionalCoefficient;
        public Fix64 FrictionalCoefficient { get { return frictionalCoefficient; } }

#if _CLIENTLOGIC_
        [BoxGroup("����˯��")]
        [LabelWidth(50)]
        [PropertyOrder(11)]
#endif
        public bool IsCanSleep = true;
#if _CLIENTLOGIC_
        [BoxGroup("˯��")]
        [LabelWidth(50)]
        [PropertyOrder(11)]
#endif
        public bool IsSleep = false;
#if _CLIENTLOGIC_
        [BoxGroup("����")]
        [LabelWidth(50)]
        [PropertyOrder(12)]
#endif
        public bool IsRun = true;
        /// <summary>
        /// ���Բ�����ײ
        /// </summary>
#if _CLIENTLOGIC_
        [BoxGroup("������ײ")]
        [LabelWidth(50)]
        [PropertyOrder(13)]
#endif
        public bool CanCollider = true;
        #endregion
#if _CLIENTLOGIC_
        [HideInInspector]
#endif
        public List<FixCollider> colliderList = new List<FixCollider>();
        public void AddCollider(FixCollider collider)
        {
            colliderList.Add(collider);
            collider.fixRigidbody = this;
        }
        public void RemoveCollider(FixCollider collider)
        {
            colliderList.Remove(collider);
            collider.fixRigidbody = null;
        }

        #region �����ٶ���λ��
        protected void UpdateAcceleration()
        {
            acceleration = netForce / mass;
        }

        protected virtual void FixedUpdateVelocity()
        {
            velocity = velocity * linearDrag + acceleration * FixPhysicsGlobal.fixedDeltaTime;
            velocity += impulse / mass;
            impulse = FixVector3.zero;
            velocity = positionLimitation * velocity;
        }

        protected virtual void FixedUpdatePosition()
        {
            fixTransform.Position += velocity * FixPhysicsGlobal.fixedDeltaTime + Fix64.ahalf * acceleration * FixPhysicsGlobal.fixedDeltaTimeSq;
        }
        #endregion
        #region ���½��ٶ�����ת
        /// <summary>
        /// ���½Ǽ��ٶ�
        /// </summary>
        protected void FixedUpdateAngularAcceleration()
        {
            angularAcceleration = inertia.Transpose() * netTorque;
        }
        protected virtual void FixedUpdateAngularVelocity()
        {
            angularVelocity = angularVelocity * rotateDrag + angularAcceleration * FixPhysicsGlobal.fixedDeltaTime;
            FixMatrix3x3 Rotation = FixTransform.Rotation.ToMatrix();
            FixMatrix3x3 I = (Rotation * Inertia * Rotation.Transpose()).Inverse();
            angularVelocity += I * angularImpulse;
            angularImpulse = FixVector3.zero;
            if (angularVelocity.MagnitudeSq() > (maxAngularVelocity).Sq())
            {
                angularVelocity = angularVelocity.Normalize() * maxAngularVelocity;
            }
          //  angularVelocity = rotationLimitation * angularVelocity;
        }
        protected virtual void FixedUpdateRotation()
        {
            if (angularVelocity == FixVector3.zero) return;
            FixQuaternion rotationChange = new FixQuaternion(Fix64.zero, angularVelocity * Fix64.ahalf * FixPhysicsGlobal.fixedDeltaTime);
          // FixQuaternion f = (rotationChange * fixTransform.Rotation);
         //   Debug.Log(rotationChange + "||"+f + "||" + f.Normalize() + $"w{f.w.value}   x{f.x.value}   y{f.y.value}    z{f.z.value}");
            fixTransform.Rotation += (rotationChange * fixTransform.Rotation);
        }
        #endregion
        /// <summary>
        /// ��Ӻ㶨��
        /// </summary>
        public void AddNetForce(FixVector3 force)
        {
            netForce += force;
        }
        public void AddNetTorque(FixVector3 force)
        {
            this.netTorque += force;
        }
        /// <summary>
        /// ��ӳ���
        /// </summary>
        public void AddImpulse(FixVector3 impulse)
        {
            this.impulse += impulse;
        }
        public void AddAngularImpulse(FixVector3 impulse)
        {
            this.angularImpulse += impulse;
        }


        public void FixedUpdate()
        {
            if (IsSleep) return;
            FixedUpdateAngularAcceleration();
            FixedUpdateAngularVelocity();
            FixedUpdateRotation();
            UpdateAcceleration();
            FixedUpdateVelocity();
            FixedUpdatePosition();

            CheckSleep();
        }

        private void CheckSleep()
        {
            if (velocity.MagnitudeSq() < FixPhysicsGlobal.sleepVelocityThreshold
                && acceleration.MagnitudeSq() < FixPhysicsGlobal.sleepVelocityThreshold
                && angularVelocity.MagnitudeSq() < FixPhysicsGlobal.sleepAngularVelocityThreshold
                && angularAcceleration.MagnitudeSq() < FixPhysicsGlobal.sleepAngularVelocityThreshold
                && IsCanSleep)
            {
                IsSleep = true;
            }
        }

        public void Init()
        {
            CalculateCenterOfMass();
            CalculateInertia();
        }

        private Fix64 totalVolume = Fix64.zero;
        /// <summary>
        /// ������������λ��
        /// </summary>
        /// <returns></returns>
        public void CalculateCenterOfMass()
        {
            FixVector3 totalPosition = FixVector3.zero;
            totalVolume = Fix64.zero;

            foreach (FixCollider collider in colliderList)
            {
                collider.CalculateVolume();
                totalPosition += collider.Position * collider.Volume;
                totalVolume += collider.Volume;
            }

            if (totalVolume != Fix64.zero)
            {
                centroid = (totalPosition / totalVolume) - FixTransform.Position;
            }
            else
            {
                // ���������Ϊ�㣬�򷵻�Ĭ�ϵ�����λ�ã�����ԭ�㣩
                centroid = FixVector3.zero;
            }
        }

        /// <summary>
        /// ����������ת����������
        /// </summary>
        private void CalculateInertia()
        {
            //FixMatrix3x3 totalInertia = new FixMatrix3x3();
            //foreach (FixCollider collider in colliderList)
            //{
            //    // ��ȡ��ײ������ڸ������������ϵ��ת������
            //    FixMatrix3x3 rotationMatrix = collider.Inertia * collider.LocalRotation.ToMatrix();
            //    // ת�����������������ϵ��
            //    FixMatrix3x3 transformedInertia = rotationMatrix.Transpose() * collider.Inertia * rotationMatrix;
            //    // ����ƽ���ᶨ������ת����������
            //    FixVector3 offset = collider.LocalPosition - centroid;
            //    FixMatrix3x3 correctionMatrix = CalculateParallelAxisTheoremCorrectionMatrix(collider.Mass, offset);
            //    // �����ײ����������ת����������
            //    totalInertia += transformedInertia + correctionMatrix;
            //}
            //inertia = totalInertia;
            inertia = new FixMatrix3x3();

            foreach (FixCollider collider in colliderList)
            {
                Fix64 m = collider.Volume / totalVolume * Mass;
                FixVector3 r = collider.Position - (FixTransform.Position + Centrold);
                collider.CalculateInertia();
                // ����ײ����ת����������ϲ����������ת������������
                inertia += CalculateParallelAxisTheoremCorrectionMatrix(collider.Inertia, m, r);
            }
            Debug.Log(FixTransform.transform.name + inertia);
        }
        /// <summary>
        /// ����ƽ���ᶨ�����������
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private FixMatrix3x3 CalculateParallelAxisTheoremCorrectionMatrix(FixMatrix3x3 Inertia, Fix64 m, FixVector3 r)
        {
            Fix64 xsq = r.x.Sq();
            Fix64 ysq = r.y.Sq();
            Fix64 zsq = r.z.Sq();
            return new FixMatrix3x3(
                Inertia.m00 + ysq + zsq, Fix64.zero, Fix64.zero,
                Fix64.zero, Inertia.m11 + xsq + zsq, Fix64.zero,
                Fix64.zero, Fix64.zero, Inertia.m22 + xsq + ysq) * m;
        }




        public event Action<FixCollisionData> OnCollisionEnter;
        public event Action<FixCollisionData> OnCollisionStay;
        public event Action<FixCollisionData> OnCollisionExit;

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

        public void ResetVelocity()
        {
            acceleration = FixVector3.zero;
            velocity = FixVector3.zero;
            impulse = FixVector3.zero;
            angularAcceleration = FixVector3.zero;
            angularVelocity = FixVector3.zero;
            angularImpulse = FixVector3.zero;
        }
    }
}
