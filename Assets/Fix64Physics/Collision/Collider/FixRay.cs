using Fix64Physics.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fix64Physics.Collision
{
    public struct FixRay
    {
        /// <summary>
        /// 起点
        /// </summary>
        public FixVector3 origin;
        /// <summary>
        /// 方向
        /// </summary>
        public FixVector3 direction;

        public FixRay(FixVector3 origin, FixVector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }
    }
}
