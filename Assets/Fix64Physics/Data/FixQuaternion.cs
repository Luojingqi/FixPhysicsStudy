using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fix64Physics.Data
{
    public struct FixQuaternion : IEquatable<FixQuaternion>
    {
        public static readonly FixQuaternion identity = new FixQuaternion(Fix64.one, Fix64.zero, Fix64.zero, Fix64.zero);

        public Fix64 w;
        public Fix64 x;
        public Fix64 y;
        public Fix64 z;

        public FixQuaternion(Fix64 w, Fix64 x, Fix64 y, Fix64 z)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public FixQuaternion(Fix64 w, FixVector3 v)
        {
            this.w = w;
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        #region 基础运算
        public static FixQuaternion operator +(FixQuaternion a, FixQuaternion b)
        {
            return new FixQuaternion(a.w + b.w, a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static FixQuaternion operator -(FixQuaternion a, FixQuaternion b)
        {
            return new FixQuaternion(a.w - b.w, a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static FixQuaternion operator *(FixQuaternion a, FixQuaternion b)
        {
            Fix64 w = a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z;
            Fix64 x = a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y;
            Fix64 y = a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x;
            Fix64 z = a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w;
            return new FixQuaternion(w, x, y, z);
        }
        public static FixQuaternion operator *(FixQuaternion a, Fix64 b)
        {
            return new FixQuaternion(a.w * b, a.x * b, a.y * b, a.z * b);
        }
        public static FixQuaternion operator *(Fix64 b, FixQuaternion a)
        {
            return new FixQuaternion(a.w * b, a.x * b, a.y * b, a.z * b);
        }
        public static FixQuaternion operator /(FixQuaternion a, Fix64 b)
        {
            return new FixQuaternion(a.w / b, a.x / b, a.y / b, a.z / b);
        }
        public static FixQuaternion operator /(Fix64 b, FixQuaternion a)
        {
            return new FixQuaternion(a.w / b, a.x / b, a.y / b, a.z / b);
        }
        /// <summary>
        /// 求逆
        /// </summary>
        /// <returns></returns>
        public FixQuaternion Inverse()
        {
            return new FixQuaternion(w, -x, -y, -z);
        }

        public static FixVector3 operator *(FixQuaternion rotation, FixVector3 point)
        {
            return rotation.ToMatrix() * point;
        }
        public static FixVector3 operator *(FixVector3 point, FixQuaternion rotation)
        {
            return rotation * point;
        }

        public static bool operator ==(FixQuaternion lhs, FixQuaternion rhs)
        {
            return lhs.w == rhs.w && lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(FixQuaternion lhs, FixQuaternion rhs)
        {
            return lhs.w != rhs.w || lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
        }
        #endregion

        public static explicit operator UnityEngine.Quaternion(FixQuaternion value)
        {
            return new UnityEngine.Quaternion((float)value.x, (float)value.y, (float)value.z, (float)value.w);
        }

        /// <summary>
        /// 将四元数转换为旋转矩阵
        /// </summary>
        /// <returns>旋转矩阵</returns>
        public FixMatrix3x3 ToMatrix()
        {
            FixMatrix3x3 rotationMatrix = new FixMatrix3x3();

            Fix64 xx = x * x;
            Fix64 yy = y * y;
            Fix64 zz = z * z;
            Fix64 xy = x * y;
            Fix64 xz = x * z;
            Fix64 yz = y * z;
            Fix64 wx = w * x;
            Fix64 wy = w * y;
            Fix64 wz = w * z;

            Fix64 two = new Fix64(2);

            rotationMatrix.m00 = Fix64.one - two * (yy + zz);
            rotationMatrix.m01 = two * (xy - wz);
            rotationMatrix.m02 = two * (xz + wy);
            rotationMatrix.m10 = two * (xy + wz);
            rotationMatrix.m11 = Fix64.one - two * (xx + zz);
            rotationMatrix.m12 = two * (yz - wx);
            rotationMatrix.m20 = two * (xz - wy);
            rotationMatrix.m21 = two * (yz + wx);
            rotationMatrix.m22 = Fix64.one - two * (xx + yy);

            return rotationMatrix;
        }

        /// <summary>
        /// 将四元数归一化
        /// </summary>
        public FixQuaternion Normalize()
        {
            Fix64 magnitude = Magnitude();
            FixQuaternion ret = this;
            if (magnitude != Fix64.zero)
            {
                Fix64 invMagnitude = Fix64.one / magnitude;
                ret.w *= invMagnitude;
                ret.x *= invMagnitude;
                ret.y *= invMagnitude;
                ret.z *= invMagnitude;
            }
            return ret;
        }

        /// <summary>
        /// 计算四元数的模长
        /// </summary>
        /// <returns>模长</returns>
        public Fix64 Magnitude()
        {
            return (w * w + x * x + y * y + z * z).Sqrt();
        }
        /// <summary>
        /// 计算四元数的模长
        /// </summary>
        /// <returns>模长</returns>
        public Fix64 MagnitudeSq()
        {
            return w * w + x * x + y * y + z * z;
        }

        public static FixQuaternion Euler(Fix64 x, Fix64 y, Fix64 z)
        {
            FixMatrix3x3 Rz = new FixMatrix3x3(
                Fix64.CosA(z), Fix64.SinA(z), Fix64.zero,
                -Fix64.SinA(z), Fix64.CosA(z), Fix64.zero,
                Fix64.zero, Fix64.zero, Fix64.one);
            FixMatrix3x3 Ry = new FixMatrix3x3(
                Fix64.CosA(y), Fix64.zero, -Fix64.SinA(y),
                Fix64.zero, Fix64.one, Fix64.zero,
                Fix64.SinA(y), Fix64.zero, Fix64.CosA(y));
            FixMatrix3x3 Rx = new FixMatrix3x3(
               Fix64.one, Fix64.zero, Fix64.zero,
               Fix64.zero, Fix64.CosA(x), Fix64.SinA(x),
               Fix64.zero, -Fix64.SinA(x), Fix64.CosA(x));
            FixMatrix3x3 R = Rz * Ry * Rx;
            Fix64 qw = (Fix64.one + R.m00 + R.m11 + R.m22).Sqrt() / 2;
            Fix64 qx = (R.m21 - R.m12) / (4 * qw);
            Fix64 qy = (R.m02 - R.m20) / (4 * qw);
            Fix64 qz = (R.m10 - R.m01) / (4 * qw);
            return new FixQuaternion(qw, qx, qy, qz);
        }

        #region 继承公共方法
        public override int GetHashCode()
        {
            return w.GetHashCode() + x.GetHashCode() + y.GetHashCode() + z.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("qw:{0} qx:{1} y:{2} z:{3}", w, x, y, z);
        }

        public bool Equals(FixQuaternion other)
        {
            return this == other;
        }
        #endregion
    }
}
