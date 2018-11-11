using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Breeding
{
    /// <summary>
    /// 种羊鉴定
    /// </summary>
    public class AssessStudsheep : Assess
    {
        /// <summary>
        /// 配种能力(公羊)
        /// </summary>
        public float MatingAbility { get; set; }
    }
}
