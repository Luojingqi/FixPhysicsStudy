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
    /// 刚体
    /// </summary>
    public class FixRigidbody
    {

        private IFixTransform fixTransform;
        public IFixTransform FixTransform { get { return fixTransform; } set { fixTransform = value; } }

        #region 基础数据
#if _CLIENTLOGIC_
        public Transform transform { get { return fixTransform.transform; } }
        [BoxGroup("静态的")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(0)]
#endif
        public bool IsStatic;

#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup("质量")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(1)]
#endif
        /// <summary>
        /// 质量
        /// </summary>
        private Fix64 mass = Fix64.one;
        public Fix64 Mass { get { return mass; } set { mass = value; } }
#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup("速度")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(2)]
#endif
        /// <summary>
        /// 速度
        /// </summary>
        private FixVector3 velocity;
        public FixVector3 Velocity { get { return velocity; } set { velocity = value; } }
#if _CLIENTLOGIC_
        [OdinSerialize]
        [HideInInspector]
#endif
        /// <summary>
        /// 加速度
        /// </summary>
        private FixVector3 acceleration;
        public FixVector3 Acceleration { get { return acceleration; } }
#if _CLIENTLOGIC_
        [HideInInspector]
        [OdinSerialize]
#endif
        /// <summary>
        /// 冲量
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
        [BoxGroup("使用重力")]
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
        [BoxGroup(@"阻尼\线性")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(6)]
#endif
        /// <summary>
        /// 阻尼
        /// </summary>
        private Fix64 linearDrag = FixPhysicsGlobal.linearDrag;
        public Fix64 LinearDrag { get { return linearDrag; } }
#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup(@"阻尼\旋转")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(7)]
#endif
        /// <summary>
        /// 阻尼
        /// </summary>
        private Fix64 rotateDrag = FixPhysicsGlobal.rotateDrag;
        public Fix64 RotateDrag { get { return rotateDrag; } }

#if _CLIENTLOGIC_
        [HideInInspector]
        [OdinSerialize]
#endif
        /// <summary>
        /// 合外力
        /// </summary>
        private FixVector3 netForce;
        public FixVector3 NetForce { get { return netForce; } set { netForce = value; } }


#if _CLIENTLOGIC_
        [OdinSerialize]
        [HideInInspector]
#endif
        /// <summary>
        /// 角加速度
        /// </summary>
        private FixVector3 angularAcceleration;
        public FixVector3 AngularAcceleration { get { return angularAcceleration; } }
#if _CLIENTLOGIC_
        [PropertyOrder(3)]
        [OdinSerialize]
        [BoxGroup("角速度")]
        [HideLabel]
        [LabelWidth(50)]
#endif
        /// <summary>
        /// 角速度
        /// </summary>
        private FixVector3 angularVelocity;
        public FixVector3 AngularVelocity { get { return angularVelocity; } }

#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup("最大角速度(弧度)")]
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
        /// 转动惯量矩阵
        /// </summary>
        public FixMatrix3x3 Inertia { get { return inertia; } set { inertia = value; } }
#if _CLIENTLOGIC_
        [HideInInspector]
        [OdinSerialize]
#endif
        private FixVector3 netTorque;
        /// <summary>
        /// 扭力
        /// </summary>
        public FixVector3 NetTorque { get { return netTorque; } set { netTorque = value; } }

#if _CLIENTLOGIC_
        [HideInInspector]
        [OdinSerialize]
#endif
        /// <summary>
        /// 角冲量
        /// </summary>
        private FixVector3 angularImpulse;
        public FixVector3 AngularImpulse { get { return angularImpulse; } }


#if _CLIENTLOGIC_
        [OdinSerialize]
        [HideInInspector]
#endif
        /// <summary>
        /// 旋转限制
        /// </summary>
        private FixMatrix3x3 rotationLimitation = FixMatrix3x3.identity;

#if _CLIENTLOGIC_
        [Button("设置旋转限制")]
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
        /// 移动限制
        /// </summary>
        private FixMatrix3x3 positionLimitation = FixMatrix3x3.identity;
#if _CLIENTLOGIC_
        [Button("设置移动限制")]
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
        [BoxGroup("质心")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(5)]
#endif
        private FixVector3 centroid;
        /// <summary>
        /// 质心(局部坐标)
        /// </summary>
        public FixVector3 Centrold { get { return centroid; } set { centroid = value; } }


#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup("弹性系数")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(8)]
#endif
        /// <summary>
        /// 弹性系数
        /// </summary>
        private Fix64 elasticCoefficient = FixPhysicsGlobal.elasticCoefficient;
        public Fix64 ElasticCoefficient { get { return elasticCoefficient; } }
#if _CLIENTLOGIC_
        [OdinSerialize]
        [BoxGroup("摩擦系数")]
        [HideLabel]
        [LabelWidth(50)]
        [PropertyOrder(9)]
#endif
        private Fix64 frictionalCoefficient = FixPhysicsGlobal.frictionalCoefficient;
        public Fix64 FrictionalCoefficient { get { return frictionalCoefficient; } }

#if _CLIENTLOGIC_
        [BoxGroup("可以睡眠")]
        [LabelWidth(50)]
        [PropertyOrder(11)]
#endif
        public bool IsCanSleep = true;
#if _CLIENTLOGIC_
        [BoxGroup("睡眠")]
        [LabelWidth(50)]
        [PropertyOrder(11)]
#endif
        public bool IsSleep = false;
#if _CLIENTLOGIC_
        [BoxGroup("运行")]
        [LabelWidth(50)]
        [PropertyOrder(12)]
#endif
        public bool IsRun = true;
        /// <summary>
        /// 可以产生碰撞
        /// </summary>
#if _CLIENTLOGIC_
        [BoxGroup("产生碰撞")]
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

        #region 更新速度与位置
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
        #region 更新角速度与旋转
        /// <summary>
        /// 更新角加速度
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
        /// 添加恒定力
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
        /// 添加冲量
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
        /// 计算刚体的质心位置
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
                // 如果总质量为零，则返回默认的质心位置（例如原点）
                centroid = FixVector3.zero;
            }
        }

        /// <summary>
        /// 计算刚体的总转动惯量矩阵
        /// </summary>
        private void CalculateInertia()
        {
            //FixMatrix3x3 totalInertia = new FixMatrix3x3();
            //foreach (FixCollider collider in colliderList)
            //{
            //    // 获取碰撞体相对于刚体的整体坐标系的转换矩阵
            //    FixMatrix3x3 rotationMatrix = collider.Inertia * collider.LocalRotation.ToMatrix();
            //    // 转换到刚体的整体坐标系中
            //    FixMatrix3x3 transformedInertia = rotationMatrix.Transpose() * collider.Inertia * rotationMatrix;
            //    // 根据平行轴定理修正转动惯量矩阵
            //    FixVector3 offset = collider.LocalPosition - centroid;
            //    FixMatrix3x3 correctionMatrix = CalculateParallelAxisTheoremCorrectionMatrix(collider.Mass, offset);
            //    // 组合碰撞体的修正后的转动惯量矩阵
            //    totalInertia += transformedInertia + correctionMatrix;
            //}
            //inertia = totalInertia;
            inertia = new FixMatrix3x3();

            foreach (FixCollider collider in colliderList)
            {
                Fix64 m = collider.Volume / totalVolume * Mass;
                FixVector3 r = collider.Position - (FixTransform.Position + Centrold);
                collider.CalculateInertia();
                // 将碰撞器的转动惯量矩阵合并到刚体的总转动惯量矩阵中
                inertia += CalculateParallelAxisTheoremCorrectionMatrix(collider.Inertia, m, r);
            }
            Debug.Log(FixTransform.transform.name + inertia);
        }
        /// <summary>
        /// 计算平行轴定理的修正矩阵
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
