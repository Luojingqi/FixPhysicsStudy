using System;

namespace Fix64Physics.Data
{
    /// <summary>
    /// 定点向量
    /// </summary>
    public struct FixVector3 : IEquatable<FixVector3>
    {
        public static readonly FixVector3 zero = new FixVector3();
        public static readonly FixVector3 one = new FixVector3(Fix64.one, Fix64.one, Fix64.one);
        public static readonly FixVector3 up = new FixVector3(Fix64.zero, Fix64.one, Fix64.zero);
        public static readonly FixVector3 down = new FixVector3(Fix64.zero, -Fix64.one, Fix64.zero);
        public static readonly FixVector3 left = new FixVector3(-Fix64.one, Fix64.zero, Fix64.zero);
        public static readonly FixVector3 right = new FixVector3(Fix64.one, Fix64.zero, Fix64.zero);
        public static readonly FixVector3 forward = new FixVector3(Fix64.zero, Fix64.zero, Fix64.one);
        public static readonly FixVector3 back = new FixVector3(Fix64.zero, Fix64.zero, -Fix64.one);
        public Fix64 x;
        public Fix64 y;
        public Fix64 z;

        public FixVector3(Fix64 x, Fix64 y, Fix64 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public FixVector3(Fix64 x, Fix64 y)
        {
            this.x = x;
            this.y = y;
            this.z = Fix64.zero;
        }
        public FixVector3(int x, int y, int z)
        {
            this = new FixVector3(new Fix64(x), new Fix64(y), new Fix64(z));
        }
        public FixVector3(int x, int y)
        {
            this = new FixVector3(new Fix64(x), new Fix64(y), Fix64.zero);
        }
        public FixVector3(long x, long y, long z)
        {
            this = new FixVector3(new Fix64(x), new Fix64(y), new Fix64(z));
        }
        public FixVector3(long x, long y)
        {
            this = new FixVector3(new Fix64(x), new Fix64(y), Fix64.zero);
        }
        public FixVector3(FixVector3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        public Fix64 this[int index]
        {
            get
            {
                if (index == 0)
                    return x;
                else if (index == 1)
                    return y;
                else
                    return z;
            }
            set
            {
                if (index == 0)
                    x = value;
                else if (index == 1)
                    y = value;
                else
                    z = value;
            }
        }

        public static FixVector3 Zero
        {
            get { return new FixVector3(Fix64.zero, Fix64.zero, Fix64.zero); }
        }

        #region 基础运算
        public static FixVector3 operator +(FixVector3 a, FixVector3 b)
        {
            Fix64 x = a.x + b.x;
            Fix64 y = a.y + b.y;
            Fix64 z = a.z + b.z;
            return new FixVector3(x, y, z);
        }
        public static FixVector3 operator +(FixVector3 a, Fix64 b)
        {
            Fix64 x = a.x + b;
            Fix64 y = a.y + b;
            Fix64 z = a.z + b;
            return new FixVector3(x, y, z);
        }

        public static FixVector3 operator -(FixVector3 a, FixVector3 b)
        {
            Fix64 x = a.x - b.x;
            Fix64 y = a.y - b.y;
            Fix64 z = a.z - b.z;
            return new FixVector3(x, y, z);
        }
        public static FixVector3 operator -(FixVector3 a, Fix64 b)
        {
            Fix64 x = a.x - b;
            Fix64 y = a.y - b;
            Fix64 z = a.z - b;
            return new FixVector3(x, y, z);
        }

        public static FixVector3 operator *(Fix64 d, FixVector3 a)
        {
            Fix64 x = a.x * d;
            Fix64 y = a.y * d;
            Fix64 z = a.z * d;
            return new FixVector3(x, y, z);
        }


        public static FixVector3 operator *(FixVector3 a, Fix64 d)
        {
            Fix64 x = a.x * d;
            Fix64 y = a.y * d;
            Fix64 z = a.z * d;
            return new FixVector3(x, y, z);
        }
        public static FixVector3 operator *(int d, FixVector3 a)
        {
            Fix64 x = a.x * d;
            Fix64 y = a.y * d;
            Fix64 z = a.z * d;
            return new FixVector3(x, y, z);
        }
        public static FixVector3 operator *(FixVector3 a, int d)
        {
            Fix64 x = a.x * d;
            Fix64 y = a.y * d;
            Fix64 z = a.z * d;
            return new FixVector3(x, y, z);
        }
        /// <summary>
        /// 向量均乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FixVector3 operator *(FixVector3 a, FixVector3 b)
        {
            return new FixVector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static FixVector3 operator /(FixVector3 a, Fix64 d)
        {
            Fix64 x = a.x / d;
            Fix64 y = a.y / d;
            Fix64 z = a.z / d;
            return new FixVector3(x, y, z);
        }
        public static FixVector3 operator /(FixVector3 a, FixVector3 b)
        {
            return new FixVector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static FixVector3 operator -(FixVector3 v)
        {
            return new FixVector3(-v.x, -v.y, -v.z);
        }

        public static bool operator ==(FixVector3 lhs, FixVector3 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(FixVector3 lhs, FixVector3 rhs)
        {
            return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
        }
        #endregion

        #region 强制转换
        public static explicit operator FixVector2(FixVector3 value)
        {
            return new FixVector2(value.x, value.y);
        }
#if _CLIENTLOGIC_
        public static explicit operator UnityEngine.Vector3(FixVector3 value)
        {
            return new UnityEngine.Vector3((float)value.x, (float)value.y, (float)value.z);
        }
        public static explicit operator UnityEngine.Vector2(FixVector3 value)
        {
            return new UnityEngine.Vector2((float)value.x, (float)value.y);
        }
        public static explicit operator FixVector3(UnityEngine.Vector3 value)
        {
            return new FixVector3((Fix64)value.x, (Fix64)value.y, (Fix64)value.z);
        }
#endif
        #endregion
        /// <summary>
        /// 求距离的平方
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Fix64 DistanceSq(FixVector3 a, FixVector3 b)
        {
            FixVector3 dir = a - b;
            return dir.x.Sq() + dir.y.Sq() + dir.z.Sq();

        }
        public static Fix64 Distance(FixVector3 a, FixVector3 b)
        {
            FixVector3 dir = a - b;
            return dir.Magnitude();

        }
        /// <summary>
        /// 补间
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FixVector3 Lerp(FixVector3 a, FixVector3 b, Fix64 t)
        {
            if (t >= Fix64.one) return b;
            FixVector3 dir = b - a;
            return a + dir * t;
        }
        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public FixVector3 Round(int n = 0)
        {
            return new FixVector3(this.x.Round(n), this.y.Round(n), this.z.Round(n));
        }
        public FixVector3 Normalize()
        {
            Fix64 m = Magnitude();
            if (m > 0) return this / m;
            else return FixVector3.zero;
        }
        /// <summary>
        /// 求模长
        /// </summary>
        /// <returns></returns>
        public Fix64 Magnitude()
        {
            return (this.x.Sq() + this.y.Sq() + this.z.Sq()).Sqrt();
        }
        /// <summary>
        /// 求模长平方
        /// </summary>
        /// <returns></returns>
        public Fix64 MagnitudeSq()
        {
            return this.x.Sq() + this.y.Sq() + this.z.Sq();
        }
        public FixMatrix3x3 ToCrossMatrix()
        {
            return new FixMatrix3x3(
                Fix64.zero, -z, y,
                z, Fix64.zero, -x,
                -y, x, Fix64.zero
                );

        }
        /// <summary>
        /// 点乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Fix64 Dot(FixVector3 a, FixVector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }
        /// <summary>
        /// 叉乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FixVector3 Cross(FixVector3 a, FixVector3 b)
        {
            Fix64 x = a.y * b.z - a.z * b.y;
            Fix64 y = a.z * b.x - a.x * b.z;
            Fix64 z = a.x * b.y - a.y * b.x;

            return new FixVector3(x, y, z);
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}", x, y, z);
        }


        public override int GetHashCode()
        {
            return this.x.GetHashCode() + this.y.GetHashCode() + this.z.GetHashCode();
        }

        public bool Equals(FixVector3 other)
        {
            return other == this;
        }

    }
}
