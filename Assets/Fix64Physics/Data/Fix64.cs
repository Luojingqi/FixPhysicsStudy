using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

namespace Fix64Physics.Data
{
#if _CLIENTLOGIC_
    [InlineProperty]
#endif
    /// <summary>
    /// 定点数
    /// </summary>
    public struct Fix64 : IEquatable<Fix64>, IComparable<Fix64>
    {
#if _CLIENTLOGIC_
        [OdinSerialize]
        [HorizontalGroup]
        [PropertyOrder(1)]
        [LabelWidth(30)]
        [HideLabel]
#endif
        public readonly long value;
        #region unity编辑器扩展
#if UNITY_EDITOR
        [HideLabel]
        [ShowInInspector]
        [HorizontalGroup]
        [PropertyOrder(0)]
        public float OdinShowValue { get { return (float)this; } }
        [HorizontalGroup]
        [PropertyOrder(2)]
        [Button("设置值", ButtonStyle.Box)]
        public void OdinSetValue([LabelText("整数")] int a = 0, [LabelText("小数")] int b = 0)
        {
            int i = 10;
            int temp = Math.Abs(b);
            while (temp > 10)
            {
                temp %= 10;
                i *= 10;
            }
            int s = Math.Sign(a);
            if (s == 0) s = 1;
            this = new Fix64(a) + new Fix64(b, i) * s;
        }
#endif
        #endregion

        public static readonly Fix64 one = new Fix64(ONE);
        public static readonly Fix64 zero = new Fix64();
        //3.1416015625
        public static readonly Fix64 PI = new Fix64(12868L);
        public static readonly Fix64 ahalf = new Fix64(1, 2);
        public static readonly Fix64 third = new Fix64(1, 3);

        public static readonly Fix64 min = new Fix64(1, 4096);
        public static readonly Fix64 max = new Fix64(long.MaxValue);

        //0.455322265625 用于四舍五入
        private static readonly Fix64 round = new Fix64(911, 2000);

        public const int FRACTIONAL_PLACES = 12;
        const long ONE = 1L << FRACTIONAL_PLACES;
        /// <summary>
        /// 不进行转换，直接设置定点数的值
        /// </summary>
        /// <param name="value"></param>
        public Fix64(long value)
        {
            this.value = value;
        }
        public Fix64(int value)
        {
            this.value = value * ONE;
        }
        public Fix64(int a, int b)
        {
            this.value = (new Fix64(a) / new Fix64(b)).value;
        }

        #region 基础运算
        public static Fix64 operator +(Fix64 x, Fix64 y)
        {
            return new Fix64(x.value + y.value);
        }
        public static Fix64 operator +(Fix64 x, int y)
        {
            return x + (Fix64)y;
        }

        public static Fix64 operator +(int x, Fix64 y)
        {
            return (Fix64)x + y;
        }
        public static Fix64 operator +(Fix64 x, float y)
        {
            return x + (Fix64)y;
        }

        public static Fix64 operator +(float x, Fix64 y)
        {
            return (Fix64)x + y;
        }
        public static Fix64 operator +(Fix64 x, double y)
        {
            return x + (Fix64)y;
        }

        public static Fix64 operator +(double x, Fix64 y)
        {
            return (Fix64)x + y;
        }

        public static Fix64 operator -(Fix64 x, Fix64 y)
        {
            return new Fix64(x.value - y.value);
        }

        public static Fix64 operator -(Fix64 x, int y)
        {
            return x - (Fix64)y;
        }

        public static Fix64 operator -(int x, Fix64 y)
        {
            return (Fix64)x - y;
        }

        public static Fix64 operator -(Fix64 x, float y)
        {
            return x - (Fix64)y;
        }

        public static Fix64 operator -(float x, Fix64 y)
        {
            return (Fix64)x + y;
        }

        public static Fix64 operator -(Fix64 x, double y)
        {
            return x - (Fix64)y;
        }

        public static Fix64 operator -(double x, Fix64 y)
        {
            return (Fix64)x - y;
        }

        public static Fix64 operator *(Fix64 x, Fix64 y)
        {
            return new Fix64((x.value * y.value) >> FRACTIONAL_PLACES);
        }

        public static Fix64 operator *(Fix64 x, int y)
        {
            return x * (Fix64)y;
        }

        public static Fix64 operator *(int x, Fix64 y)
        {
            return (Fix64)x * y;
        }

        public static Fix64 operator *(Fix64 x, float y)
        {
            return x * (Fix64)y;
        }

        public static Fix64 operator *(float x, Fix64 y)
        {
            return (Fix64)x * y;
        }

        public static Fix64 operator *(Fix64 x, double y)
        {
            return x * (Fix64)y;
        }

        public static Fix64 operator *(double x, Fix64 y)
        {
            return (Fix64)x * y;
        }
        public static Fix64 operator /(Fix64 x, Fix64 y)
        {
            return new Fix64((x.value << FRACTIONAL_PLACES) / y.value);
        }

        public static Fix64 operator /(Fix64 x, int y)
        {
            return x / (Fix64)y;
        }

        public static Fix64 operator /(int x, Fix64 y)
        {
            return (Fix64)x / y;
        }

        public static Fix64 operator /(Fix64 x, float y)
        {
            return x / (Fix64)y;
        }

        public static Fix64 operator /(float x, Fix64 y)
        {
            return (Fix64)x / y;
        }

        public static Fix64 operator /(double x, Fix64 y)
        {
            return (Fix64)x / y;
        }

        public static Fix64 operator /(Fix64 x, double y)
        {
            return x / (Fix64)y;
        }

        public static Fix64 operator %(Fix64 x, Fix64 y)
        {
            return new Fix64(x.value % y.value);
        }

        public static Fix64 operator -(Fix64 x)
        {
            return new Fix64(-x.value);
        }

        public static bool operator ==(Fix64 x, Fix64 y)
        {
            return x.value == y.value;
        }

        public static bool operator !=(Fix64 x, Fix64 y)
        {
            return x.value != y.value;
        }

        public static bool operator >(Fix64 x, Fix64 y)
        {
            return x.value > y.value;
        }

        public static bool operator >(Fix64 x, int y)
        {
            return x > (Fix64)y;
        }
        public static bool operator <(Fix64 x, int y)
        {
            return x < (Fix64)y;
        }
        public static bool operator >(Fix64 x, float y)
        {
            return x > (Fix64)y;
        }
        public static bool operator <(Fix64 x, float y)
        {
            return x < (Fix64)y;
        }

        public static bool operator <(Fix64 x, Fix64 y)
        {
            return x.value < y.value;
        }

        public static bool operator >=(Fix64 x, Fix64 y)
        {
            return x.value >= y.value;
        }

        public static bool operator <=(Fix64 x, Fix64 y)
        {
            return x.value <= y.value;
        }

        public static Fix64 operator >>(Fix64 x, int amount)
        {
            return new Fix64(x.value >> amount);
        }

        public static Fix64 operator <<(Fix64 x, int amount)
        {
            return new Fix64(x.value << amount);
        }

        #endregion

        #region 强制转换
        public static explicit operator Fix64(long value)
        {
            return new Fix64(value * ONE);
        }
        public static explicit operator long(Fix64 value)
        {
            return value.value >> FRACTIONAL_PLACES;
        }
        public static explicit operator Fix64(float value)
        {
            return new Fix64((long)(value * ONE));
        }
        public static explicit operator float(Fix64 value)
        {
            return (float)value.value / ONE;
        }
        public static explicit operator int(Fix64 value)
        {
            return (int)((float)value);
        }
        public static explicit operator Fix64(int value)
        {
            return new Fix64(value);
        }
        public static explicit operator Fix64(double value)
        {
            return new Fix64((long)(value * ONE));
        }
        public static explicit operator double(Fix64 value)
        {
            return (double)value.value / ONE;
        }
        public static explicit operator Fix64(decimal value)
        {
            return new Fix64((long)(value * ONE));
        }
        public static explicit operator decimal(Fix64 value)
        {
            return (decimal)value.value / ONE;
        }
        #endregion

        #region 三角函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">角度</param>
        /// <returns></returns>
        public static Fix64 SinA(Fix64 x)
        {
            return Sin(x * Fix64.PI / new Fix64(180));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">弧度</param>
        /// <returns></returns>
        public static Fix64 Sin(Fix64 x)
        {
            while (x > Fix64.PI)
                x -= Fix64.PI * 2;
            while (x < -Fix64.PI)
                x += Fix64.PI * 2;
            Fix64 result = Fix64.zero;
            Fix64 term = x;
            Fix64 sign = Fix64.one;

            int n = 3;
            for (int i = 1; i <= n * 2; i += 2)
            {
                result += sign * term / Factorial(i);
                term *= x.Sq();
                sign = -sign;
            }

            return result;
        }
        private static Fix64 Factorial(int n)
        {
            if (n <= 1)
                return Fix64.one;

            Fix64 result = Fix64.one;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        /// <summary>
        /// 求cos
        /// </summary>
        /// <param name="x">弧度</param>
        /// <returns></returns>
        public static Fix64 Cos(Fix64 x)
        {
            return Sin(x + Fix64.PI / 2);
        }
        /// <summary>
        /// 求cos
        /// </summary>
        /// <param name="x">角度</param>
        /// <returns></returns>
        public static Fix64 CosA(Fix64 x)
        {
            return SinA(x + new Fix64(90));
        }
        #endregion

        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        public Fix64 Round(int round = 0)
        {
            Fix64 factor = new Fix64(10);
            factor = Fix64.Pow(factor, round);
            //记录符号，当a为负数的时候，左右移动后会比正常少一
            Fix64 f = this.Sign();
            //整数部分
            Fix64 a = (this * factor) * f;
            a = a >> FRACTIONAL_PLACES;
            a = a << FRACTIONAL_PLACES;
            //小数部分
            Fix64 d = this * f - a;
            a += d >= Fix64.round ? Fix64.one : Fix64.zero;
            Fix64 roundedValue = a / factor * f;
            return roundedValue;
        }
        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Fix64 Pow(Fix64 x, int y)
        {
            if (y == 0) return Fix64.one;
            Fix64 fix = x;
            for (int i = 1; i < y; i++)
            {
                x = x * fix;
            }

            return x;
        }
        /// <summary>
        /// 判断正负，返回1,0,-1
        /// </summary>
        /// <returns></returns>
        public Fix64 Sign()
        {
            if (value > 0) return Fix64.one;
            else if (value < 0) return -Fix64.one;
            else return Fix64.zero;
        }
        /// <summary>
        /// 范围限制
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Fix64 Clamp(Fix64 min, Fix64 max)
        {
            if (min > max)
            {
                min += max;
                max = min - max;
                min -= max;
            }
            if (this < min) return min;
            else if (this > max) return max;
            else return this;

        }
        /// <summary>
        /// 绝对值
        /// </summary>
        /// <returns></returns>
        public Fix64 Abs()
        {
            return this * Sign();
        }
        /// <summary>
        /// 求平方
        /// </summary>
        /// <returns></returns>
        public Fix64 Sq()
        {
            return this * this;
        }
        /// <summary>
        /// 开平方根
        /// </summary>
        /// <returns></returns>
        public Fix64 Sqrt()
        {
            if (this == Fix64.zero) return Fix64.zero;
            Fix64 guess = this > Fix64.one ? this * Fix64.ahalf : this;
            Fix64 prevGuess;
            //误差，控制迭代次数
            Fix64 error = Fix64.zero;
            int i = 0;
            do
            {
                i++;
                prevGuess = guess;
                guess = (guess + this / guess) * Fix64.ahalf;
            } while ((prevGuess - guess).Abs() > error && i < 10);

            return guess;
        }
        public static Fix64 Min(Fix64 a, Fix64 b)
        {
            return a < b ? a : b;
        }
        public static Fix64 Min(Fix64 a, Fix64 b, out Fix64 max)
        {
            Fix64 min;
            if (a < b)
            {
                min = a;
                max = b;
            }
            else
            {
                min = b;
                max = a;
            }
            return min;
        }
        public static Fix64 Max(Fix64 a, Fix64 b)
        {
            return a > b ? a : b;
        }



        #region 继承公共方法
        public int CompareTo(Fix64 other)
        {
            return value.CompareTo(other.value);
        }

        public bool Equals(Fix64 other)
        {
            return ((Fix64)other).value == value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return ((decimal)this).ToString();
        }
        #endregion

    }

    //public class VerticalFieldDrawer : OdinValueDrawer<int>
    //{
    //    protected override void DrawPropertyLayout(GUIContent label)
    //    {
    //        Rect rect = EditorGUILayout.GetControlRect();
    //        EditorGUI.LabelField(rect, label);

    //        rect.y += EditorGUIUtility.singleLineHeight;

    //        this.ValueEntry.SmartValue = EditorGUI.IntField(rect, this.ValueEntry.SmartValue);
    //    }
    //}
}
