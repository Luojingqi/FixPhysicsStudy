using Fix64Physics.Data;
using System.Drawing;
using UnityEngine;

namespace Fix64Physics.Collision
{
    /// <summary>
    /// AxisAlignedBoundingBox
    /// aabb包围盒
    /// </summary>
    public struct AABB
    {
        public FixVector3 min;
        public FixVector3 max;
        public FixVector3 Center { get { return (min + max) / new Fix64(2); } }

        public bool Intersect(AABB b)
        {
            return !(max.x < b.min.x || max.y < b.min.y || max.z < b.min.z
                || min.x > b.max.x || min.y > b.max.y || min.z > b.max.z);
        }

        public Fix64 GetDistance(FixVector3 start, FixVector3 end)
        {
            Fix64 tMin = Fix64.zero;
            Fix64 tMax = Fix64.one;

            FixVector3 delta = end - start;

            for (int i = 0; i < 2; ++i)
            {
                if (delta[i] != Fix64.zero)
                {
                    Fix64 dMin = (min[i] - start[i]) / delta[i];
                    Fix64 dMax = (max[i] - start[i]) / delta[i];

                    if (dMin > dMax)
                    {
                        Fix64 temp = dMin;
                        dMin = dMax;
                        dMax = temp;
                    }

                    tMin = Fix64.Max(tMin, dMin);
                    tMax = Fix64.Min(tMax, dMax);
                }
                else if (start[i] < min[i] || start[i] > max[i]) //起点不在包围盒范围。
                    return Fix64.max;
            }

            if (tMin > tMax || tMax < 0 || tMin > 1)
            {
                return Fix64.max;
            }

            return tMin;
        }

        /// <summary>
        /// 获得松散的包围盒。只要在误差范围内，就不需要更新AABB树
        /// </summary>
        /// <returns></returns>
        public AABB GetLooseBounds()
        {
            AABB ret = this;
            ret.min = ret.min - Fix64.third;
            ret.max = ret.max + Fix64.third;
            return ret;
        }
        public Fix64 GetDiagonalSq()
        {
            FixVector3 size;
            size.x = this.max.x - this.min.x;
            size.y = this.max.y - this.min.y;
            size.z = this.max.z - this.min.z;
            return size.x.Sq() + size.y.Sq() + size.z.Sq();
        }
        public static AABB operator +(AABB a, AABB b)
        {
            AABB ret;
            ret.max.x = Fix64.Max(a.max.x, b.max.x);
            ret.max.y = Fix64.Max(a.max.y, b.max.y);
            ret.max.z = Fix64.Max(a.max.z, b.max.z);
            ret.min.x = Fix64.Min(a.min.x, b.min.x);
            ret.min.y = Fix64.Min(a.min.y, b.min.y);
            ret.min.z = Fix64.Min(a.min.z, b.min.z);
            return ret;
        }
        public static AABB operator +(AABB a, FixVector3 b)
        {
            AABB ret;
            ret.max.x = a.max.x + b.x;
            ret.max.y = a.max.y + b.y;
            ret.max.z = a.max.z + b.z;
            ret.min.x = a.min.x + b.x;
            ret.min.y = a.min.y + b.y;
            ret.min.z = a.min.z + b.z;
            return ret;
        }
        public static AABB operator *(AABB a, FixQuaternion b)
        {
            AABB ret;
            FixMatrix3x3 rMatrix = b.ToMatrix();
            FixVector3 v0 = a.min;
            FixVector3 v1 = a.max;
            FixVector3 v2 = new FixVector3(a.min.x, a.min.y, a.max.z);
            FixVector3 v3 = new FixVector3(a.max.x, a.min.y, a.max.z);
            FixVector3 v4 = new FixVector3(a.min.x, a.max.y, a.max.z);
            FixVector3 v5 = new FixVector3(a.min.x, a.max.y, a.min.z);
            FixVector3 v6 = new FixVector3(a.max.x, a.max.y, a.min.z);
            FixVector3 v7 = new FixVector3(a.max.x, a.min.y, a.min.z);
            FixVector3 r0 = rMatrix * v0;
            FixVector3 r1 = rMatrix * v1;
            FixVector3 r2 = rMatrix * v2;
            FixVector3 r3 = rMatrix * v3;
            FixVector3 r4 = rMatrix * v4;
            FixVector3 r5 = rMatrix * v5;
            FixVector3 r6 = rMatrix * v6;
            FixVector3 r7 = rMatrix * v7;
            #region 找出最大最小点
            ret.min.x = Fix64.max;
            ret.min.x = Fix64.Min(ret.min.x, r0.x);
            ret.min.x = Fix64.Min(ret.min.x, r1.x);
            ret.min.x = Fix64.Min(ret.min.x, r2.x);
            ret.min.x = Fix64.Min(ret.min.x, r3.x);
            ret.min.x = Fix64.Min(ret.min.x, r4.x);
            ret.min.x = Fix64.Min(ret.min.x, r5.x);
            ret.min.x = Fix64.Min(ret.min.x, r6.x);
            ret.min.x = Fix64.Min(ret.min.x, r7.x);
            ret.min.y = Fix64.max;
            ret.min.y = Fix64.Min(ret.min.y, r0.y);
            ret.min.y = Fix64.Min(ret.min.y, r1.y);
            ret.min.y = Fix64.Min(ret.min.y, r2.y);
            ret.min.y = Fix64.Min(ret.min.y, r3.y);
            ret.min.y = Fix64.Min(ret.min.y, r4.y);
            ret.min.y = Fix64.Min(ret.min.y, r5.y);
            ret.min.y = Fix64.Min(ret.min.y, r6.y);
            ret.min.y = Fix64.Min(ret.min.y, r7.y);
            ret.min.z = Fix64.max;
            ret.min.z = Fix64.Min(ret.min.z, r0.z);
            ret.min.z = Fix64.Min(ret.min.z, r1.z);
            ret.min.z = Fix64.Min(ret.min.z, r2.z);
            ret.min.z = Fix64.Min(ret.min.z, r3.z);
            ret.min.z = Fix64.Min(ret.min.z, r4.z);
            ret.min.z = Fix64.Min(ret.min.z, r5.z);
            ret.min.z = Fix64.Min(ret.min.z, r6.z);
            ret.min.z = Fix64.Min(ret.min.z, r7.z);
            ret.max.x = Fix64.min;
            ret.max.x = Fix64.Max(ret.max.x, r0.x);
            ret.max.x = Fix64.Max(ret.max.x, r1.x);
            ret.max.x = Fix64.Max(ret.max.x, r2.x);
            ret.max.x = Fix64.Max(ret.max.x, r3.x);
            ret.max.x = Fix64.Max(ret.max.x, r4.x);
            ret.max.x = Fix64.Max(ret.max.x, r5.x);
            ret.max.x = Fix64.Max(ret.max.x, r6.x);
            ret.max.x = Fix64.Max(ret.max.x, r7.x);
            ret.max.y = Fix64.min;
            ret.max.y = Fix64.Max(ret.max.y, r0.y);
            ret.max.y = Fix64.Max(ret.max.y, r1.y);
            ret.max.y = Fix64.Max(ret.max.y, r2.y);
            ret.max.y = Fix64.Max(ret.max.y, r3.y);
            ret.max.y = Fix64.Max(ret.max.y, r4.y);
            ret.max.y = Fix64.Max(ret.max.y, r5.y);
            ret.max.y = Fix64.Max(ret.max.y, r6.y);
            ret.max.y = Fix64.Max(ret.max.y, r7.y);
            ret.max.z = Fix64.min;
            ret.max.z = Fix64.Max(ret.max.z, r0.z);
            ret.max.z = Fix64.Max(ret.max.z, r1.z);
            ret.max.z = Fix64.Max(ret.max.z, r2.z);
            ret.max.z = Fix64.Max(ret.max.z, r3.z);
            ret.max.z = Fix64.Max(ret.max.z, r4.z);
            ret.max.z = Fix64.Max(ret.max.z, r5.z);
            ret.max.z = Fix64.Max(ret.max.z, r6.z);
            ret.max.z = Fix64.Max(ret.max.z, r7.z);
            #endregion
            return ret;
        }
        public override string ToString()
        {
            return $"min:{min}\tmax:{max}";
        }
    }
}
