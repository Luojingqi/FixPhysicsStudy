using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Fix64Physics.Data
{
    public struct FixMatrix4x4
    {
        public static FixMatrix4x4 orthogonal = new FixMatrix4x4(
            Fix64.one, Fix64.zero, Fix64.zero, Fix64.zero,
            Fix64.zero, Fix64.one, Fix64.zero, Fix64.zero,
            Fix64.zero, Fix64.zero, Fix64.one, Fix64.zero,
            Fix64.zero, Fix64.zero, Fix64.zero, Fix64.one);

        public Fix64 m00, m01, m02, m03;
        public Fix64 m10, m11, m12, m13;
        public Fix64 m20, m21, m22, m23;
        public Fix64 m30, m31, m32, m33;

        public FixMatrix4x4(Fix64 m00, Fix64 m01, Fix64 m02, Fix64 m03,
                                Fix64 m10, Fix64 m11, Fix64 m12, Fix64 m13,
                                Fix64 m20, Fix64 m21, Fix64 m22, Fix64 m23,
                                Fix64 m30, Fix64 m31, Fix64 m32, Fix64 m33)
        {
            this.m00 = m00; this.m01 = m01; this.m02 = m02; this.m03 = m03;
            this.m10 = m10; this.m11 = m11; this.m12 = m12; this.m13 = m13;
            this.m20 = m20; this.m21 = m21; this.m22 = m22; this.m23 = m23;
            this.m30 = m30; this.m31 = m31; this.m32 = m32; this.m33 = m33;
        }

        // 矩阵乘法
        public static FixMatrix4x4 operator *(FixMatrix4x4 a, FixMatrix4x4 b)
        {
            FixMatrix4x4 result = new FixMatrix4x4();
            result.m00 = a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30;
            result.m01 = a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31;
            result.m02 = a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32;
            result.m03 = a.m00 * b.m03 + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33;

            result.m10 = a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20 + a.m13 * b.m30;
            result.m11 = a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31;
            result.m12 = a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32;
            result.m13 = a.m10 * b.m03 + a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33;

            result.m20 = a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20 + a.m23 * b.m30;
            result.m21 = a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31;
            result.m22 = a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32;
            result.m23 = a.m20 * b.m03 + a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33;

            result.m30 = a.m30 * b.m00 + a.m31 * b.m10 + a.m32 * b.m20 + a.m33 * b.m30;
            result.m31 = a.m30 * b.m01 + a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31;
            result.m32 = a.m30 * b.m02 + a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32;
            result.m33 = a.m30 * b.m03 + a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33;

            return result;
        }

        // 矩阵加法
        public static FixMatrix4x4 operator +(FixMatrix4x4 a, FixMatrix4x4 b)
        {
            FixMatrix4x4 result = new FixMatrix4x4();
            result.m00 = a.m00 + b.m00;
            result.m01 = a.m01 + b.m01;
            result.m02 = a.m02 + b.m02;
            result.m03 = a.m03 + b.m03;

            result.m10 = a.m10 + b.m10;
            result.m11 = a.m11 + b.m11;
            result.m12 = a.m12 + b.m12;
            result.m13 = a.m13 + b.m13;

            result.m20 = a.m20 + b.m20;
            result.m21 = a.m21 + b.m21;
            result.m22 = a.m22 + b.m22;
            result.m23 = a.m23 + b.m23;

            result.m30 = a.m30 + b.m30;
            result.m31 = a.m31 + b.m31;
            result.m32 = a.m32 + b.m32;
            result.m33 = a.m33 + b.m33;

            return result;
        }

    }
}
