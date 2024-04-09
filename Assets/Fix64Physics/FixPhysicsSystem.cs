using Fix64Physics.Collision;
using Fix64Physics.Data;
using Fix64Physics.Mono;
using System;
using System.Collections.Generic;

namespace Fix64Physics
{
    public class FixPhysicsSystem
    {

        public static FixPhysicsSystem Inst;
        public FixPhysicsSystem()
        {
            if (Inst == null) Inst = this;

            this.fixFrameTimeLen = FixPhysicsGlobal.fixedDeltaTime;
            frameTimeLen = (float)FixPhysicsGlobal.fixedDeltaTime;
        }

        public void R()
        {
            aabbTrue.R();
        }

        AABBTree aabbTrue = new AABBTree();
        public List<FixRigidbodyMono> rigidbodyMonoList = new List<FixRigidbodyMono>();
        public List<FixCollider> fixColliderList = new List<FixCollider>();
        public void Add(FixRigidbodyMono rigidbodyMono)
        {
            rigidbodyMonoList.Add(rigidbodyMono);

            CollisionLogDic.Add(rigidbodyMono.fixRigidbody, HashSetFixRigidbodyPool.TakeOut());
            linearMoveDic.Add(rigidbodyMono.fixRigidbody, FixVector3.zero);
            EventDic.Add(rigidbodyMono.fixRigidbody, DictionaryFixColliderBoolPool.TakeOut());
            ChangeEventDic.Add(rigidbodyMono.fixRigidbody, ListFixColliderPool.TakeOut());
            RemoveEventDic.Add(rigidbodyMono.fixRigidbody, ListFixColliderPool.TakeOut());
        }
        public void Remove(FixRigidbodyMono rigidbodyMono)
        {
            rigidbodyMonoList.Remove(rigidbodyMono);
            var hashSet = CollisionLogDic[rigidbodyMono.fixRigidbody];
            hashSet.Clear();
            HashSetFixRigidbodyPool.PutIn(hashSet);
            CollisionLogDic.Remove(rigidbodyMono.fixRigidbody);

            linearMoveDic.Remove(rigidbodyMono.fixRigidbody);

            var dic = EventDic[rigidbodyMono.fixRigidbody];
            dic.Clear();
            DictionaryFixColliderBoolPool.PutIn(dic);
            EventDic.Remove(rigidbodyMono.fixRigidbody);

            var changeList = ChangeEventDic[rigidbodyMono.fixRigidbody];
            changeList.Clear();
            ListFixColliderPool.PutIn(changeList);
            ChangeEventDic.Remove(rigidbodyMono.fixRigidbody);

            var removeList = RemoveEventDic[rigidbodyMono.fixRigidbody];
            removeList.Clear();
            ListFixColliderPool.PutIn(changeList);
            RemoveEventDic.Remove(rigidbodyMono.fixRigidbody);
        }
        public void Add(FixCollider collider) { aabbTrue.Add(collider); fixColliderList.Add(collider); }
        public void Remove(FixCollider collider) { aabbTrue.Remove(collider); fixColliderList.Remove(collider); }


        /// <summary>
        /// 渲染帧时间总和
        /// </summary>
        public float renderingTimeSum { get; private set; } = 0;
        /// <summary>
        /// 设定的逻辑帧更新间隔
        /// </summary>
        public float frameTimeLen { get; private set; }
        public Fix64 fixFrameTimeLen { get; private set; }

        public bool IsRun = true;

        public void RanderUpdate(float timeTween, float frameTween, float deltaTime)
        {
            foreach (var r in rigidbodyMonoList)
                r.GameRenderPosition(timeTween, frameTween, deltaTime);
        }

        /// <summary>
        /// 记录当前帧每个刚体和哪些产生了碰撞
        /// </summary>
        private Dictionary<FixRigidbody, HashSet<FixRigidbody>> CollisionLogDic = new Dictionary<FixRigidbody, HashSet<FixRigidbody>>();
        private ObjectPool<HashSet<FixRigidbody>> HashSetFixRigidbodyPool = new ObjectPool<HashSet<FixRigidbody>>(250, true);
        /// <summary>
        /// 当前刚体的某个碰撞体aabb与哪些碰撞体aabb相交
        /// </summary>
        private List<FixCollider> nowCollisionList = new List<FixCollider>();
        /// <summary>
        /// 刚体当前帧应该的线性移动
        /// </summary>
        private Dictionary<FixRigidbody, FixVector3> linearMoveDic = new Dictionary<FixRigidbody, FixVector3>();

        /// <summary>
        /// 用于判断当前帧应该触发哪一个事件
        /// </summary>
        private Dictionary<FixRigidbody, Dictionary<FixCollider, bool>> EventDic = new Dictionary<FixRigidbody, Dictionary<FixCollider, bool>>();
        private ObjectPool<Dictionary<FixCollider, bool>> DictionaryFixColliderBoolPool = new ObjectPool<Dictionary<FixCollider, bool>>(500, true);

        private Dictionary<FixRigidbody, List<FixCollider>> RemoveEventDic = new Dictionary<FixRigidbody, List<FixCollider>>();
        private Dictionary<FixRigidbody, List<FixCollider>> ChangeEventDic = new Dictionary<FixRigidbody, List<FixCollider>>();
        private ObjectPool<List<FixCollider>> ListFixColliderPool = new ObjectPool<List<FixCollider>>(500, true);
        //private Dictionary<>
        public void PhysicsUpdate()
        {
            OnPhysicsUpdateEnter?.Invoke();
            for (int i = 0; i < rigidbodyMonoList.Count; i++)
            {
                FixRigidbody nowRigidbody = rigidbodyMonoList[i].fixRigidbody;
                if (nowRigidbody.IsSleep || nowRigidbody.IsRun == false) continue;
                if (nowRigidbody.CanCollider)
                    for (int j = 0; j < nowRigidbody.colliderList.Count; j++)
                    {
                        aabbTrue.CollisionQuery(nowRigidbody.colliderList[j], ref nowCollisionList);
                        for (int k = 0; k < nowCollisionList.Count; k++)
                        {
                            if (nowCollisionList[k].fixRigidbody == nowRigidbody ||
                                CollisionLogDic[nowRigidbody].Contains(nowCollisionList[k].fixRigidbody))
                                continue;
                            else
                            {
                                FixCollision collision0 = new FixCollision();
                                FixCollision collision1 = new FixCollision();
                                if (CollisionDetection.Try(nowRigidbody.colliderList[j], nowCollisionList[k], ref collision0, ref collision1))
                                {
                                    #region 碰撞数据初始化
                                    //质心指向碰撞点的向量
                                    FixVector3 r0 = (collision0.position - (nowRigidbody.Centrold + nowRigidbody.FixTransform.Position));
                                    FixVector3 nowAV0 = FixVector3.Cross(nowRigidbody.AngularVelocity, r0);
                                    FixVector3 nowLV0 = nowRigidbody.Velocity;
                                    FixVector3 nowV0 = nowAV0 + nowLV0;
                                    #endregion
                                    FixVector3 nowLV1 = FixVector3.zero, nowAV1 = FixVector3.zero, r1 = FixVector3.zero, nowV1 = FixVector3.zero;
                                    FixRigidbody otherRigidbody = nowCollisionList[k].fixRigidbody;
                                    //是否是与其他刚体碰撞
                                    bool beOther = false;
                                    if (otherRigidbody != null)
                                    {
                                        if (otherRigidbody.IsRun == false || nowRigidbody.CanCollider == false) continue;
                                        //唤醒被碰撞刚体
                                        otherRigidbody.IsSleep = false;
                                        #region 被碰撞数据初始化
                                        beOther = true;
                                        r1 = (collision1.position - (otherRigidbody.Centrold + otherRigidbody.FixTransform.Position));
                                        nowLV1 = otherRigidbody.Velocity;
                                        nowAV1 = FixVector3.Cross(otherRigidbody.AngularVelocity, r1);
                                        nowV1 = nowLV1 + nowAV1;
                                        #endregion
                                    }
                                    FixVector3 nowVelocity = nowV0 - nowV1;
                                    if (FixVector3.Dot(nowVelocity, collision0.normal) > 0)
                                    {
                                        //当前处于分离状态，跳过
                                        continue;
                                    }

                                    #region 触发碰撞事件
                                    FixCollisionData data0 = new FixCollisionData(collision0, nowRigidbody.colliderList[j], nowCollisionList[k]);
                                    FixCollisionData data1 = new FixCollisionData(collision1, nowCollisionList[k], nowRigidbody.colliderList[j]);
                                    if (EventDic[nowRigidbody].ContainsKey(nowCollisionList[k]))
                                    {
                                        //当前刚体和这个碰撞箱有过碰撞，触发Stay事件
                                        nowRigidbody.OnCollisionStayInvoke(data0);
                                        //最后检测事件字典，如果值为false说明当前帧没碰撞触发Exit事件
                                        EventDic[nowRigidbody][nowCollisionList[k]] = true;
                                        if (beOther)
                                        {
                                            //被碰撞碰撞箱也是刚体
                                            otherRigidbody.OnCollisionStayInvoke(data1);
                                        }
                                        else
                                        {
                                            if (nowCollisionList[k].IsTrigger)
                                            {
                                                nowCollisionList[k].OnTriggerStayInvoke(data1);
                                                continue;
                                            }
                                            else
                                                nowCollisionList[k].OnCollisionStayInvoke(data1);
                                        }

                                    }
                                    else
                                    {
                                        //当前刚体未与这个碰撞箱有过碰撞，触发Enter事件
                                        nowRigidbody.OnCollisionEnterInvoke(data0);
                                        EventDic[nowRigidbody].Add(nowCollisionList[k], true);
                                        if (beOther)
                                        {
                                            //被碰撞碰撞箱也是刚体
                                            otherRigidbody.OnCollisionEnterInvoke(data1);
                                        }
                                        else
                                        {
                                            if (nowCollisionList[k].IsTrigger)
                                            {
                                                nowCollisionList[k].OnTriggerEnterInvoke(data1);
                                                continue;
                                            }
                                            else
                                                nowCollisionList[k].OnCollisionEnterInvoke(data1);
                                        }
                                    }
                                    #endregion

                                    #region 计算碰撞冲量
                                    FixVector3 nowVelocityN = FixVector3.Dot(nowVelocity, collision0.normal) * collision0.normal;
                                    Fix64 elastic = Fix64.ahalf * (nowRigidbody.ElasticCoefficient + (beOther ? otherRigidbody.ElasticCoefficient : nowRigidbody.ElasticCoefficient));
                                    FixVector3 newVelocityN = -nowVelocityN * elastic;

                                    FixVector3 nowVelocityT = nowVelocity - nowVelocityN;
                                    Fix64 tM = nowVelocityT.Magnitude();
                                    Fix64 frictional = Fix64.ahalf * (nowRigidbody.FrictionalCoefficient + (beOther ? otherRigidbody.FrictionalCoefficient : nowRigidbody.FrictionalCoefficient));
                                    Fix64 a = Fix64.one;
                                    if (tM != Fix64.zero)
                                        a = Fix64.Max(Fix64.zero, 1 - frictional * (1 + elastic) * nowVelocityN.Magnitude() / tM);
                                    FixVector3 newVelocityT = a * nowVelocityT;
                                    FixVector3 VDifference = newVelocityN + newVelocityT - nowVelocity;
                                    //Debug.Log(nnn + nowRigidbody.transform.name + otherRigidbody?.transform.name + $"v0{nowV0}\tv1{nowV1}\tnV{nowVelocity}\tvD{VDifference}\tel{elastic}");

                                    FixMatrix3x3 Rotation0 = nowRigidbody.FixTransform.Rotation.ToMatrix();
                                    FixMatrix3x3 I_inverse0 = (Rotation0 * nowRigidbody.Inertia * Rotation0.Transpose()).Inverse();
                                    FixMatrix3x3 rCross0 = r0.ToCrossMatrix();
                                    FixMatrix3x3 K0 = FixMatrix3x3.identity * (Fix64.one / nowRigidbody.Mass) - rCross0 * I_inverse0 * rCross0;
                                    FixVector3 J0 = K0.Inverse() * VDifference;

                                    nowRigidbody.AddImpulse(J0);
                                    nowRigidbody.AddAngularImpulse(FixVector3.Cross(r0, J0));

                                    linearMoveDic[nowRigidbody] += collision0.normal * collision0.depth;
                                    #endregion
                                    if (beOther)
                                    {
                                        #region 计算被碰撞冲量
                                        FixMatrix3x3 Rotation1 = otherRigidbody.FixTransform.Rotation.ToMatrix();
                                        FixMatrix3x3 I_inverse1 = (Rotation1 * otherRigidbody.Inertia * Rotation1.Transpose()).Inverse();
                                        FixMatrix3x3 rCross1 = r1.ToCrossMatrix();
                                        FixMatrix3x3 K1 = FixMatrix3x3.identity * (Fix64.one / otherRigidbody.Mass) - rCross1 * I_inverse1 * rCross1;
                                        FixVector3 J1 = K1.Inverse() * -VDifference;
                                        otherRigidbody.AddImpulse(J1);
                                        otherRigidbody.AddAngularImpulse(FixVector3.Cross(r1, J1));

                                        linearMoveDic[otherRigidbody] += collision1.normal * collision1.depth;
                                        #endregion
                                        CollisionLogDic[nowRigidbody].Add(otherRigidbody);
                                        CollisionLogDic[otherRigidbody].Add(nowRigidbody);
                                    }
                                    #region debug
                                    //Debug.Log(nnn + "\n" +
                                    //    $"r0\t{r0}\n" +
                                    //    $"角速度X{FixVector3.Cross(r0, nowRigidbody.AngularVelocity)}\n" +
                                    //    $"nowV\t{nowVelocity}\n" +
                                    //    $"nowVN\t{nowVelocityN}\n" +
                                    //    $"nowVT\t{nowVelocityT}\n" +
                                    //    $"newVN\t{newVelocityN}\n" +
                                    //    $"newVT\t{newVelocityT}\n" +
                                    //    $"K\n{K}\n" +
                                    //    $"J\t{J}\n" +
                                    //    $"Rotation\n{Rotation}" +
                                    //    $"a{a}");
                                    //nnn++;
                                    // Debug.Log(rigidbodyMonoList[i].fixRigidbodyMono.FixTransform.transform.name + "碰撞" + nowCollisionList[k].fixTransform.transform.name);
                                    #endregion
                                }
                            }
                        }
                        nowCollisionList.Clear();
                    }
                rigidbodyMonoList[i].PhysicsUpdate();
                OnPhysicsUpdateExit?.Invoke();
            }
            //检查aabb树是否需要更新
            aabbTrue.CheckUpdate();
            //更新需要线性平移的刚体的位置
            foreach (var log in CollisionLogDic)
            {
                //设置局部或全局坐标时会造成误差，比如设置好一个局部坐标值，然后会更新全局坐标，将这个全局坐标重新赋值给自身，更新得到的局部坐标值与最开始的局部坐标不一致
                //故当小于一定值时我们不更新坐标，防止误差累计导致刚体坐标产生持续性偏移
               // if (linearMoveDic[log.Key].MagnitudeSq() <= FixPhysicsGlobal.linearMoveThreshold) continue;
                log.Key.FixTransform.Position += linearMoveDic[log.Key];
                linearMoveDic[log.Key] = FixVector3.zero;
                log.Value.Clear();
            }

            #region 触发碰撞事件或检测事件
            for (int i = 0; i < rigidbodyMonoList.Count; i++)
            {
                FixRigidbody nowRigidbody = rigidbodyMonoList[i].fixRigidbody;
                foreach (var d in EventDic[nowRigidbody])
                {
                    if (d.Value == false)
                    {
                        //当前帧刚体未与碰撞箱发送碰撞
                        nowRigidbody.OnCollisionExitInvoke(new FixCollisionData(new FixCollision(), nowRigidbody.colliderList[0], d.Key));
                        if (d.Key.IsTrigger)
                        {
                            d.Key.OnTriggerExitInvoke(new FixCollisionData(new FixCollision(), d.Key, nowRigidbody.colliderList[0]));
                        }
                        else if (d.Key.fixRigidbody == null)
                        {
                            d.Key.OnCollisionExitInvoke(new FixCollisionData(new FixCollision(), d.Key, nowRigidbody.colliderList[0]));
                        }

                        RemoveEventDic[nowRigidbody].Add(d.Key);
                    }
                    else
                    {
                        ChangeEventDic[nowRigidbody].Add(d.Key);
                    }
                }
            }
            #endregion
            #region 检查碰撞事件是否需要被移除
            foreach (var dic in ChangeEventDic)
            {
                foreach (var l in dic.Value)
                    EventDic[dic.Key][l] = false;
                dic.Value.Clear();
            }
            foreach (var dic in RemoveEventDic)
            {
                foreach (var l in dic.Value)
                    EventDic[dic.Key].Remove(l);
                dic.Value.Clear();
            }
            #endregion
        }

        public void Update()
        {

            if (!IsRun) return;
            float deltaTime = 0;
#if _CLIENTLOGIC_
            deltaTime = UnityEngine.Time.deltaTime;
#else
            deltaTime = frameTimeLen;
#endif
            renderingTimeSum += deltaTime;

            while (renderingTimeSum > frameTimeLen)
            {
                PhysicsUpdate();
                //重置渲染帧
                renderingTimeSum -= frameTimeLen;
            }
            float timeTween = renderingTimeSum;
            float frameTween = renderingTimeSum / frameTimeLen;
            if (frameTween <= 1)
            {
                RanderUpdate(timeTween, frameTween, deltaTime);
            }
        }
        /// <summary>
        /// 射线检测，返回最近的一个碰撞点
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="maxDistance">最大距离</param>
        /// <param name="hit">返回碰撞数据</param>
        /// <param name="rayEvent">射线事件，当返回为false时，跳过当前碰撞数据</param>
        /// <param name="Layer">层级，只检测当前层的碰撞体</param>
        /// <returns></returns>
        public bool Raycast(FixRay ray, Fix64 maxDistance, out FixRaycastHit hit, Func<FixCollider, bool> rayEvent = null, int Layer = 0)
        {
            return aabbTrue.Raycast(ray, maxDistance, out hit, rayEvent, Layer);
        }

        public event Action OnPhysicsUpdateEnter;
        public event Action OnPhysicsUpdateExit;
    }
}
