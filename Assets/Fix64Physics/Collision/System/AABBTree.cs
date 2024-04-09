using DataStructure;
using Fix64Physics.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fix64Physics.Collision
{
    internal class AABBTree
    {

        public AABBTree()
        {
            AABBNodePool.PutInEvent += n => n.Reset();
            LastUpdateDataPool.PutInEvent += n => n.Reset();
        }
        private AABBNode root;

        /// <summary>
        /// 碰撞查询
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="visitor"></param>
        public void CollisionQuery(FixCollider collider, ref List<FixCollider> possible)
        {
            if (root == null)
            {
                return;
            }
            // Debug.Log(collider.aabb);
            RecursionQuery(root, collider.aabb, ref possible);
        }
        /// <summary>
        /// 碰撞查询
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="visitor"></param>
        public void CollisionQuery(AABB collider, ref List<FixCollider> possible)
        {
            if (root == null)
            {
                return;
            }
            RecursionQuery(root, collider, ref possible);
        }

        private void RecursionQuery(AABBNode node, AABB collider, ref List<FixCollider> possible)
        {
            if (!node.aabb.Intersect(collider)) return;

            if (node.childsNode[0] == null && node.childsNode[1] == null)
            {
                // Debug.Log(node.collider.aabb +"\t123");
                possible.Add(node.collider);
            }
            else
            {
                RecursionQuery(node.childsNode[0], collider, ref possible);
                RecursionQuery(node.childsNode[1], collider, ref possible);
            }
        }

        /// <summary>
        /// 射线拾取
        /// </summary>
        public bool Raycast(FixRay ray, Fix64 maxDistance, out FixRaycastHit hit, Func<FixCollider, bool> rayEvent, int Layer)
        {
            hit = new FixRaycastHit();
            if (root == null)
            {
                return false;
            }

            hit.distance = maxDistance;
            return Raycast(root, ray, ref hit, rayEvent, Layer);
        }

        private bool Raycast(AABBNode node, FixRay ray, ref FixRaycastHit hit, Func<FixCollider, bool> rayEvent, int Layer)
        {
            if (node.childsNode[0] == null && node.childsNode[1] == null)
            {
                if (rayEvent == null || rayEvent.Invoke(node.collider))
                    if (node.collider.RayLayer.Contains(Layer))
                        if (!node.collider.IsTrigger
                            && node.collider.Raycast(ray, out FixRaycastHit temp)
                            && temp.distance < hit.distance)
                        {
                            hit = temp;
                            return true;
                        }
                return false;
            }

            FixVector3 rayEnd = ray.origin + ray.direction * hit.distance;
            Fix64 d0 = node.childsNode[0].aabb.GetDistance(ray.origin, rayEnd);
            Fix64 d1 = node.childsNode[1].aabb.GetDistance(ray.origin, rayEnd);

            AABBNode node0 = node.childsNode[0];
            AABBNode node1 = node.childsNode[1];

            if (d0 > d1)
            {
                AABBNode tempNode = node0;
                node0 = node1;
                node1 = tempNode;

                Fix64 tempFix = d0;
                d0 = d1;
                d1 = tempFix;
            }
            if (d0 < hit.distance)
            {
                if (Raycast(node0, ray, ref hit, rayEvent, Layer)) return true;
            }
            else
            {

            }
            if (d1 < hit.distance)
            {
                if (Raycast(node1, ray, ref hit, rayEvent, Layer)) return true;
            }
            else
            {

            }
            return false;
        }


        public void R()
        {
            if (g.Count > 0)
            {
                g.ForEach(g => GameObject.Destroy(g));
                g.Clear();
            }

            if (root != null)
                A(root);
        }
        List<GameObject> g = new List<GameObject>();
        private void A(AABBNode node)
        {
            if (node.childsNode[0] == null && node.childsNode[1] == null)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = (Vector3)node.aabb.Center;
                cube.transform.localScale = (Vector3)(node.aabb.max - node.aabb.min);
                g.Add(cube);
            }
            if (node.childsNode[0] != null)
                A(node.childsNode[0]);
            if (node.childsNode[1] != null)
                A(node.childsNode[1]);
        }


        private Dictionary<FixCollider, AABBNode> fastSeekDic = new Dictionary<FixCollider, AABBNode>();
        public void Add(FixCollider collider)
        {
            if (root == null)
            {
                root = CreateNode(collider);
                return;
            }
            AABB aabb = collider.aabb.GetLooseBounds();
            AABBNode nowNode = root;
            Fix64 LDiagonalSq = Fix64.max;
            Fix64 RDiagonalSq = Fix64.max;
            while (nowNode.childsNode[0] != null || nowNode.childsNode[1] != null)
            {
                if (nowNode.childsNode[0] != null)
                    LDiagonalSq = (nowNode.childsNode[0].aabb + aabb).GetDiagonalSq();
                if (nowNode.childsNode[1] != null)
                    RDiagonalSq = (nowNode.childsNode[1].aabb + aabb).GetDiagonalSq();

                if (LDiagonalSq < RDiagonalSq)
                    nowNode = nowNode.childsNode[0];
                else if (LDiagonalSq > RDiagonalSq)
                    nowNode = nowNode.childsNode[1];
                else nowNode =
                        FixVector3.DistanceSq(nowNode.childsNode[0].aabb.Center, collider.aabb.Center)
                        < FixVector3.DistanceSq(nowNode.childsNode[1].aabb.Center, collider.aabb.Center)
                        ? nowNode.childsNode[0] : nowNode.childsNode[1];
            }
            nowNode.childsNode[0] = CreateNode(nowNode.collider, nowNode);
            nowNode.childsNode[1] = CreateNode(collider, nowNode);
            LastUpdateDataPool.PutIn(nowNode.lastData);
            lastUpdateDataList.Remove(nowNode.lastData);
            nowNode.collider = null;
            nowNode.lastData = null;
            UpdateBoundsBottomUp(nowNode);


        }
        public void Remove(FixCollider collider)
        {
            AABBNode node;
            if (fastSeekDic.TryGetValue(collider, out node)) fastSeekDic.Remove(collider);
            else return;

            if (node == root)
            {
                root = null;
                lastUpdateDataList.Remove(node.lastData);
                LastUpdateDataPool.PutIn(node.lastData);
                AABBNodePool.PutIn(node);
                return;
            }

            AABBNode parent = node.parent;
            AABBNode neighbour = node == parent.childsNode[0] ? parent.childsNode[1] : parent.childsNode[0];

            // 将邻居结点作为父结点
            AABBNode parentParent = parent.parent;
            neighbour.parent = parentParent;
            if (parentParent == null)
            {
                root = neighbour;
            }
            else
            {
                if (parent == parentParent.childsNode[0])
                {
                    parentParent.childsNode[0] = neighbour;
                }
                else
                {
                    parentParent.childsNode[1] = neighbour;
                }
                UpdateBoundsBottomUp(parentParent);
            }
            //  Debug.Log(lastUpdateDataList.Remove(parent.lastData));
            lastUpdateDataList.Remove(node.lastData);
            // LastUpdateDataPool.PutIn(parent.lastData);
            LastUpdateDataPool.PutIn(node.lastData);
            AABBNodePool.PutIn(parent);
            AABBNodePool.PutIn(node);
        }
        private AABBNode CreateNode(FixCollider collider, AABBNode parent = null)
        {
            LastUpdateData lastData = LastUpdateDataPool.TakeOut();
            lastData.collider = collider;
            lastData.lastPosition = collider.Position;
            lastData.lastQuaternion = collider.fixTransform.Rotation;
            lastUpdateDataList.Add(lastData);
            AABBNode newNode = AABBNodePool.TakeOut();
            newNode.parent = parent;
            newNode.aabb = collider.aabb.GetLooseBounds();
            newNode.collider = collider;
            newNode.lastData = lastData;
            if (fastSeekDic.ContainsKey(collider))
                fastSeekDic[collider] = newNode;
            else
                fastSeekDic.Add(collider, newNode);
            return newNode;
        }
        private void UpdateBoundsBottomUp(AABBNode node)
        {
            while (node != null)
            {
                node.aabb = node.childsNode[0].aabb + node.childsNode[1].aabb;
                node = node.parent;
            }
        }

        private void ReBuild()
        {

        }

        private void Update(FixCollider collider)
        {
            Remove(collider);
            Add(collider);
        }

        public bool ContainsKey(FixCollider key)
        {
            return fastSeekDic.ContainsKey(key);
        }

        private List<FixCollider> needUpdateList = new List<FixCollider>();
        public void CheckUpdate()
        {

            for (int i = 0; i < lastUpdateDataList.Count; i++)
            {
                if (lastUpdateDataList[i].Check())
                    needUpdateList.Add(lastUpdateDataList[i].collider);

            }
            foreach (var collider in needUpdateList)
                Update(collider);

            needUpdateList.Clear();
        }
        private List<LastUpdateData> lastUpdateDataList = new List<LastUpdateData>();
        private ObjectPool<LastUpdateData> LastUpdateDataPool = new ObjectPool<LastUpdateData>(100, true);
        private class LastUpdateData
        {
            public FixVector3 lastPosition;
            public FixQuaternion lastQuaternion;
            public FixCollider collider;

            public void Reset()
            {
                collider = null;
            }
            private static Fix64 positionUpdateSq = Fix64.ahalf.Sq();

            public bool Check()
            {
                bool update = false;

                // Debug.Log($"coll{collider. == null}   fix{collider.fixTransform == null}");
                if (FixVector3.DistanceSq(collider.Position, lastPosition) > positionUpdateSq)
                {
                    update = true;
                    lastPosition = collider.Position;
                }
                if (lastQuaternion != collider.fixTransform.Rotation)
                {
                    update = true;
                    lastQuaternion = collider.fixTransform.Rotation;
                }

                return update;

            }
        }


        private ObjectPool<AABBNode> AABBNodePool = new ObjectPool<AABBNode>(500, true);
        private class AABBNode
        {
            public AABBNode parent;
            public AABBNode[] childsNode = new AABBNode[2];
            public AABB aabb;
            public FixCollider collider;
            public LastUpdateData lastData;

            public void Reset()
            {
                parent = null;
                childsNode[0] = null;
                childsNode[1] = null;
                collider = null;
                lastData = null;
            }
        }
    }
}
