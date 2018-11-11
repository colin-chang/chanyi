using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Breeding
{
    /// <summary>
    /// 后备羊第三次鉴定
    /// </summary>
    public class ThirdAssess : Assess
    {
        /// <summary>
        /// 配种能力（公羊）
        /// </summary>
        public float MatingAbility { get; set; }
    }
}
