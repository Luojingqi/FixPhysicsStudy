using Fix64Physics.Data;
using TransformReplace;
using UnityEngine;

namespace Fix64Physics.Collision
{
    public static class CollisionDetection
    {
        public static bool Try(FixCollider colliderA, FixCollider colliderB, ref FixCollision ac, ref FixCollision bc)
        {
            switch (colliderA, colliderB)
            {
                case (FixSphereCollider a, FixSphereCollider b):
                    return Sphere_Sphere(a, b, ref ac, ref bc);
                case (FixSphereCollider a, FixPlaneCollider b):
                    return Sphere_Plane(a, b, ref ac, ref bc);
                case (FixSphereCollider a, FixBoxCollider b):
                    return Sphere_Box(a, b, ref ac, ref bc);
                case (FixBoxCollider a, FixSphereCollider b):
                    return Sphere_Box(b, a, ref bc, ref ac);
                case (FixSphereCollider a, FixCylinderCollider b):
                    return Shpere_Cylinder(a, b, ref ac, ref bc);
                case (FixCylinderCollider a, FixSphereCollider b):
                    return Shpere_Cylinder(b, a, ref bc, ref ac);
                case (FixSphereCollider a, FixCapsuleCollider b):
                    return Shpere_Capsule(a, b, ref ac, ref bc);
                case (FixCapsuleCollider a, FixSphereCollider b):
                    return Shpere_Capsule(b, a, ref bc, ref ac);
            }

            return false;
        }

        private static bool Sphere_Sphere(FixSphereCollider a, FixSphereCollider b, ref FixCollision ac, ref FixCollision bc)
        {
            FixVector3 v = a.Position - b.Position;

            Fix64 vmSq = v.MagnitudeSq();
            Fix64 abrSq = (a.R + b.R).Sq();
            if (vmSq > abrSq)
                return false;
            else
            {
                Fix64 vm = vmSq.Sqrt();
                FixVector3 vn = new FixVector3(v.x / vm, v.y / vm, v.z / vm);
                ac.normal = vn;
                bc.normal = -vn;
                ac.position = -vn * a.R + a.Position;
                bc.position = vn * b.R + b.Position;
                ac.depth = a.R + b.R - vm;
                bc.depth = ac.depth;
                return true;
            }
        }
        private static bool Sphere_Plane(FixSphereCollider a, FixPlaneCollider b, ref FixCollision ac, ref FixCollision bc)
        {
            FixVector3 pn = b.fixTransform.Rotation * FixVector3.up;
            FixVector3 aPosition = b.fixTransform.FixInverseTransformPoint(a.Position + b.CenterOffset);
            if (FixVector3.Dot(FixVector3.up, aPosition) < 0)
                return false;
            else
            {
                bc.position = new FixVector3(
                    aPosition.x.Clamp(-b.Size.x * Fix64.ahalf, b.Size.x * Fix64.ahalf),
                    Fix64.zero,
                    aPosition.z.Clamp(-b.Size.y * Fix64.ahalf, b.Size.y * Fix64.ahalf));
                bc.position = b.fixTransform.FixTransformPoint(bc.position);
                FixVector3 d = (bc.position - a.Position);
                Fix64 dsq = d.MagnitudeSq();
                if (dsq > a.R.Sq()) return false;
                Fix64 dsqrt = dsq.Sqrt();
                bc.normal = new FixVector3(d.x / dsqrt, d.y / dsqrt, d.z / dsqrt);
                ac.position = bc.normal * a.R + a.Position;
                ac.normal = -bc.normal;
                bc.depth = FixVector3.Distance(bc.position, ac.position);
                ac.depth = bc.depth;
                return true;

            }

        }
        private static bool Sphere_Box(FixSphereCollider a, FixBoxCollider b, ref FixCollision ac, ref FixCollision bc)
        {
            
            FixVector3 aPosition = b.fixTransform.FixInverseTransformPoint(a.Position + b.CenterOffset);
            bc.position = new FixVector3(
                aPosition.x.Clamp(-b.Size.x * Fix64.ahalf, b.Size.x * Fix64.ahalf),
                aPosition.y.Clamp(-b.Size.y * Fix64.ahalf, b.Size.y * Fix64.ahalf),
                aPosition.z.Clamp(-b.Size.z * Fix64.ahalf, b.Size.z * Fix64.ahalf)
                );
            bc.position = b.fixTransform.FixTransformPoint(bc.position);
            FixVector3 d = (bc.position - a.Position);
            Fix64 dsq = d.MagnitudeSq();
           // Debug.Log($"aP{a.Position}      name{a.fixTransform.transform.name}    dsq{dsq}    rsq{a.R}");
            if (dsq > a.R.Sq() || dsq == Fix64.zero) return false;
            Fix64 dsqrt = dsq.Sqrt();
            bc.normal = new FixVector3(d.x / dsqrt, d.y / dsqrt, d.z / dsqrt);
            ac.position = bc.normal * a.R + a.Position;
            ac.normal = -bc.normal;
            bc.depth = FixVector3.Distance(bc.position, ac.position);
            ac.depth = bc.depth;
            //Debug.Log(a.Position);
            return true;
        }
        private static bool Shpere_Cylinder(FixSphereCollider a, FixCylinderCollider b, ref FixCollision ac, ref FixCollision bc)
        {
            FixVector3 aPosition = b.fixTransform.FixInverseTransformPoint(a.Position + b.CenterOffset);
            bc.position.y = aPosition.y.Clamp(-b.H * Fix64.ahalf, b.H * Fix64.ahalf);
            FixVector2 d2m = new FixVector2(aPosition.x, aPosition.z);
            if (d2m.MagnitudeSq() > b.R.Sq())
            {
                d2m = d2m.Normalize() * b.R;
            }
            bc.position = new FixVector3(d2m.x, bc.position.y, d2m.y);
            bc.position = b.fixTransform.FixTransformPoint(bc.position);
            FixVector3 d = (bc.position - a.Position);
            Fix64 dsq = d.MagnitudeSq();
            if (dsq > a.R.Sq() || dsq == Fix64.zero) return false;
            Fix64 dsqrt = dsq.Sqrt();
            bc.normal = new FixVector3(d.x / dsqrt, d.y / dsqrt, d.z / dsqrt);
            ac.position = bc.normal * a.R + a.Position;
            ac.normal = -bc.normal;
            bc.depth = FixVector3.Distance(bc.position, ac.position);
            ac.depth = bc.depth;
            return true;
        }
        private static bool Shpere_Capsule(FixSphereCollider a, FixCapsuleCollider b, ref FixCollision ac, ref FixCollision bc)
        {
            FixVector3 aPosition = b.fixTransform.FixInverseTransformPoint(a.Position + b.CenterOffset);
            Fix64 h = (b.H - new Fix64(2) * b.R) * Fix64.ahalf;
            if (aPosition.y < h && aPosition.y > -h)
            {
                bc.position.y = aPosition.y.Clamp(-h, h);
                FixVector2 d2m = new FixVector2(aPosition.x, aPosition.z);
                if (d2m.MagnitudeSq() > b.R.Sq())
                {
                    d2m = d2m.Normalize() * b.R;
                }
                bc.position = new FixVector3(d2m.x, bc.position.y, d2m.y);
                bc.position = b.fixTransform.FixTransformPoint(bc.position);
                FixVector3 d = (bc.position - a.Position);
                Fix64 dsq = d.MagnitudeSq();
                if (dsq > a.R.Sq() || dsq == Fix64.zero) return false;
                Fix64 dsqrt = dsq.Sqrt();
                bc.normal = new FixVector3(d.x / dsqrt, d.y / dsqrt, d.z / dsqrt);
                ac.position = bc.normal * a.R + a.Position;
                ac.normal = -bc.normal;
                bc.depth = FixVector3.Distance(bc.position, ac.position);
                ac.depth = bc.depth;
                return true;
            }
            else
            {
                FixVector3 bP;
                if (aPosition.y > h)
                    bP = b.fixTransform.FixTransformPoint(FixVector3.up * h);
                else
                    bP = b.fixTransform.FixTransformPoint(FixVector3.down * h);
                FixVector3 v = a.Position - bP;

                Fix64 vmSq = v.MagnitudeSq();
                Fix64 abrSq = (a.R + b.R).Sq();
                if (vmSq > abrSq)
                    return false;
                else
                {
                    Fix64 vm = vmSq.Sqrt();
                    FixVector3 vn = new FixVector3(v.x / vm, v.y / vm, v.z / vm);
                    ac.normal = vn;
                    bc.normal = -vn;
                    ac.position = -vn * a.R + a.Position;
                    bc.position = vn * b.R + bP;
                    ac.depth = a.R + b.R - vm;
                    bc.depth = ac.depth;
                    return true;
                }
            }
        }


    }
}
