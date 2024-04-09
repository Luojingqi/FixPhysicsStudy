using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fix64Physics.Data
{
    public struct FixMatrix3x3
    {
        /// <summary>
        /// 单位
        /// </summary>
        public static FixMatrix3x3 identity = new FixMatrix3x3(
            Fix64.one, Fix64.zero, Fix64.zero,
            Fix64.zero, Fix64.one, Fix64.zero,
            Fix64.zero, Fix64.zero, Fix64.one);

        public Fix64 m00, m01, m02;
        public Fix64 m10, m11, m12;
        public Fix64 m20, m21, m22;

        public FixMatrix3x3(Fix64 m00, Fix64 m01, Fix64 m02,
                            Fix64 m10, Fix64 m11, Fix64 m12,
                            Fix64 m20, Fix64 m21, Fix64 m22)
        {
            this.m00 = m00; this.m01 = m01; this.m02 = m02;
            this.m10 = m10; this.m11 = m11; this.m12 = m12;
            this.m20 = m20; this.m21 = m21; this.m22 = m22;
        }
        public FixMatrix3x3(FixVector3 a, FixVector3 b, FixVector3 c)
        {
            this.m00 = a.x; this.m01 = b.x; this.m02 = c.x;
            this.m10 = a.y; this.m11 = b.y; this.m12 = c.y;
            this.m20 = a.z; this.m21 = b.z; this.m22 = c.z;
        }
        public Fix64 this[int x, int y]
        {
            get
            {
                switch (x, y)
                {
                    case (0, 0): return m00;
                    case (0, 1): return m01;
                    case (0, 2): return m02;
                    case (1, 0): return m10;
                    case (1, 1): return m11;
                    case (1, 2): return m12;
                    case (2, 0): return m20;
                    case (2, 1): return m21;
                    case (2, 2): return m22;
                    default: return m00;
                }
            }
            set
            {
                switch (x, y)
                {
                    case (0, 0): m00 = value; break;
                    case (0, 1): m01 = value; break;
                    case (0, 2): m02 = value; break;
                    case (1, 0): m10 = value; break;
                    case (1, 1): m11 = value; break;
                    case (1, 2): m12 = value; break;
                    case (2, 0): m20 = value; break;
                    case (2, 1): m21 = value; break;
                    case (2, 2): m22 = value; break;
                    default: m00 = value; break;
                }
            }
        }

        public static FixMatrix3x3 operator *(FixMatrix3x3 a, FixMatrix3x3 b)
        {
            FixMatrix3x3 result = new FixMatrix3x3();
            result.m00 = a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20;
            result.m01 = a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21;
            result.m02 = a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22;

            result.m10 = a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20;
            result.m11 = a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21;
            result.m12 = a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22;

            result.m20 = a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20;
            result.m21 = a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21;
            result.m22 = a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22;

            return result;
        }
        public static FixMatrix3x3 operator -(FixMatrix3x3 a, FixMatrix3x3 b)
        {
            FixMatrix3x3 ret = a;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    ret[i, j] -= b[i, j];
                }
            }
            return ret;
        }

        public static FixVector3 operator *(FixMatrix3x3 matrix, FixVector3 vector)
        {
            FixVector3 result = new FixVector3();
            result.x = matrix.m00 * vector.x + matrix.m01 * vector.y + matrix.m02 * vector.z;
            result.y = matrix.m10 * vector.x + matrix.m11 * vector.y + matrix.m12 * vector.z;
            result.z = matrix.m20 * vector.x + matrix.m21 * vector.y + matrix.m22 * vector.z;
            return result;
        }

        public static FixMatrix3x3 operator *(FixMatrix3x3 matrix, Fix64 a)
        {
            return new FixMatrix3x3(
                matrix.m00 * a, matrix.m01 * a, matrix.m02 * a,
                matrix.m10 * a, matrix.m11 * a, matrix.m12 * a,
                matrix.m20 * a, matrix.m21 * a, matrix.m22 * a);
        }

        public static FixMatrix3x3 operator +(FixMatrix3x3 a, FixMatrix3x3 b)
        {
            FixMatrix3x3 result = new FixMatrix3x3();
            result.m00 = a.m00 + b.m00;
            result.m01 = a.m01 + b.m01;
            result.m02 = a.m02 + b.m02;

            result.m10 = a.m10 + b.m10;
            result.m11 = a.m11 + b.m11;
            result.m12 = a.m12 + b.m12;

            result.m20 = a.m20 + b.m20;
            result.m21 = a.m21 + b.m21;
            result.m22 = a.m22 + b.m22;

            return result;
        }
        public static FixMatrix3x3 operator +(Fix64 a, FixMatrix3x3 b)
        {
            FixMatrix3x3 ret = b;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    ret[i, j] += a;
                }
            }
            return ret;
        }
        public static FixMatrix3x3 operator +(FixMatrix3x3 b, Fix64 a)
        {
            FixMatrix3x3 ret = b;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    ret[i, j] += a;
                }
            }
            return ret;
        }
        public static FixMatrix3x3 operator -(FixMatrix3x3 b, Fix64 a)
        {
            FixMatrix3x3 ret = b;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    ret[i, j] -= a;
                }
            }
            return ret;
        }
        public static FixMatrix3x3 operator -(Fix64 a, FixMatrix3x3 b)
        {
            FixMatrix3x3 ret = b;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    ret[i, j] = a - b[i, j];
                }
            }
            return ret;
        }
        public static FixMatrix3x3 operator /(Fix64 a, FixMatrix3x3 b)
        {
            FixMatrix3x3 ret = b;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    ret[i, j] = a / b[i, j];
                }
            }
            return ret;
        }

        /// <summary>
        /// 计算矩阵的转置
        /// </summary>
        /// <returns>转置后的矩阵</returns>
        public FixMatrix3x3 Transpose()
        {
            FixMatrix3x3 result = new FixMatrix3x3();

            result.m00 = m00;
            result.m01 = m10;
            result.m02 = m20;

            result.m10 = m01;
            result.m11 = m11;
            result.m12 = m21;

            result.m20 = m02;
            result.m21 = m12;
            result.m22 = m22;

            return result;
        }
        /// <summary>
        /// 顺对角矩阵求逆
        /// </summary>
        /// <returns></returns>
        public FixMatrix3x3 DiagonalInverse()
        {
            FixMatrix3x3 ret = this;
            for (int i = 0; i < 3; i++)
            {
                if (ret[i, i] != Fix64.zero)
                {
                    ret[i, i] = Fix64.one / ret[i, i];
                }
            }
            return ret;
        }

        public FixMatrix3x3 Inverse()
        {
            // 辅助变量  
            Fix64 det = Determinant();
            if (det == Fix64.zero)
            {
                throw new InvalidOperationException("Matrix is not invertible (determinant is zero).");
            }

            Fix64 invDet = Fix64.one / det;

            // 计算逆矩阵的元素  
            FixMatrix3x3 inverse = new FixMatrix3x3(
                (m11 * m22 - m12 * m21) * invDet, (m02 * m21 - m01 * m22) * invDet, (m01 * m12 - m02 * m11) * invDet,
                (m12 * m20 - m10 * m22) * invDet, (m00 * m22 - m02 * m20) * invDet, (m02 * m10 - m00 * m12) * invDet,
                (m10 * m21 - m11 * m20) * invDet, (m01 * m20 - m00 * m21) * invDet, (m00 * m11 - m01 * m10) * invDet);

            return inverse;
        }

        public Fix64 Determinant()
        {
            // 计算3x3矩阵的行列式  
            Fix64 det = m00 * (m11 * m22 - m12 * m21)
                        - m01 * (m10 * m22 - m12 * m20)
                        + m02 * (m10 * m21 - m11 * m20);
            return det;
        }
        public override string ToString()
        {
            return string.Format(
                m00 + "\t" + m01 + '\t' + m02 + '\n' +
                m10 + "\t" + m11 + '\t' + m12 + '\n' +
                m20 + "\t" + m21 + '\t' + m22 + '\n');
        }
    }
}
