using System;

namespace Fix64Physics.Data
{
    public struct FixVector2 : IEquatable<FixVector2>
    {
        public static readonly FixVector2 zero = new FixVector2();
        public static readonly FixVector2 one = new FixVector2(Fix64.one, Fix64.one);
        public static readonly FixVector2 up = new FixVector2(Fix64.zero, Fix64.one);
        public static readonly FixVector2 down = new FixVector2(Fix64.zero, -Fix64.one);
        public static readonly FixVector2 left = new FixVector2(-Fix64.one, Fix64.zero);
        public static readonly FixVector2 right = new FixVector2(Fix64.one, Fix64.zero);
        public Fix64 x;
        public Fix64 y;

        public FixVector2(Fix64 x, Fix64 y)
        {
            this.x = x;
            this.y = y;
        }

        public FixVector2(int x, int y)
        {
            this = new FixVector2(new Fix64(x), new Fix64(y));
        }

        public FixVector2(FixVector2 v)
        {
            this.x = v.x;
            this.y = v.y;
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
                    return x;
            }
            set
            {
                if (index == 0)
                    x = value;
                else if (index == 1)
                    y = value;
                else
                    y = value;
            }
        }

        public static FixVector2 Zero
        {
            get { return new FixVector2(Fix64.zero, Fix64.zero); }
        }

        #region 基础运算
        public static FixVector2 operator +(FixVector2 a, FixVector2 b)
        {
            Fix64 x = a.x + b.x;
            Fix64 y = a.y + b.y;
            return new FixVector2(x, y);
        }

        public static FixVector2 operator -(FixVector2 a, FixVector2 b)
        {
            Fix64 x = a.x - b.x;
            Fix64 y = a.y - b.y;
            return new FixVector2(x, y);
        }

        public static FixVector2 operator *(Fix64 d, FixVector2 a)
        {
            Fix64 x = a.x * d;
            Fix64 y = a.y * d;
            return new FixVector2(x, y);
        }

        public static FixVector2 operator *(FixVector2 a, Fix64 d)
        {
            Fix64 x = a.x * d;
            Fix64 y = a.y * d;
            return new FixVector2(x, y);
        }

        public static FixVector2 operator /(FixVector2 a, Fix64 d)
        {
            Fix64 x = a.x / d;
            Fix64 y = a.y / d;
            return new FixVector2(x, y);
        }

        public static FixVector2 operator -(FixVector2 v)
        {
            return new FixVector2(-v.x, -v.y);
        }

        public static bool operator ==(FixVector2 lhs, FixVector2 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(FixVector2 lhs, FixVector2 rhs)
        {
            return lhs.x != rhs.x || lhs.y != rhs.y;
        }
        #endregion

        #region 强制转换
        public static explicit operator FixVector3(FixVector2 value)
        {
            return new FixVector3(value.x, value.y, Fix64.zero);
        }
#if _CLIENTLOGIC_
        public static explicit operator UnityEngine.Vector3(FixVector2 value)
        {
            return new UnityEngine.Vector3((float)value.x, (float)value.y, 0);
        }
        public static explicit operator UnityEngine.Vector2(FixVector2 value)
        {
            return new UnityEngine.Vector2((float)value.x, (float)value.y);
        }
        public static explicit operator FixVector2(UnityEngine.Vector2 value)
        {
            return new FixVector2((Fix64)value.x, (Fix64)value.y);
        }
#endif
        #endregion
        /// <summary>
        /// 求距离的平方
        /// </summary>
        public static Fix64 DistanceSq(FixVector2 a, FixVector2 b)
        {
            FixVector2 dir = a - b;
            return dir.x.Sq() + dir.y.Sq();

        }
        public static FixVector2 Lerp(FixVector2 a, FixVector2 b, Fix64 t)
        {
            FixVector2 dir = b - a;
            return a + dir * t;
        }

        public FixVector2 Normalize()
        {
            Fix64 m = Magnitude();
            if (m > 0) return this / m;
            else return FixVector2.zero;
        }
        /// <summary>
        /// 求模长
        /// </summary>
        /// <returns></returns>
        public Fix64 Magnitude()
        {
            return (this.x.Sq() + this.y.Sq()).Sqrt();
        }
        /// <summary>
        /// 求模长平方
        /// </summary>
        /// <returns></returns>
        public Fix64 MagnitudeSq()
        {
            return this.x.Sq() + this.y.Sq();
        }

        #region 继承公共方法
        public override string ToString()
        {
            return string.Format("x:{0} y:{1}", x, y);
        }


        public override int GetHashCode()
        {
            return this.x.GetHashCode() + this.y.GetHashCode();
        }

        public bool Equals(FixVector2 other)
        {
            return other == this;
        }
#endregion

    }
}
