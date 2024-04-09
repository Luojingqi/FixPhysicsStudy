using Fix64Physics.Data;
using FrameLogicFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TransformReplace
{
    /// <summary>
    /// transform替换接口
    /// </summary>
    public interface IFixTransform
    {
        public Transform transform { get; }
        public IFixTransform Parent { get; set; }
        public List<IFixTransform> ChildList { get; }
        public FixVector3 Position { get; set; }
        public FixVector3 LocalPosition { get; set; }
        public FixQuaternion Rotation { get; set; }
        public FixQuaternion LocalRotation { get; set; }
        public FixVector3 LocalScale { get; set; }
        public FixVector3 LossyScale { get; }
    }
}
