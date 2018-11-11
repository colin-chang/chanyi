using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Breeding
{
    /// <summary>
    /// 后备羊第二次鉴定
    /// </summary>
    public class SecondAssess : Assess
    {
        /// <summary>
        /// 品种特征评分
        /// </summary>
        public float BreedFeatureScore { get; set; }

        /// <summary>
        /// 生殖器官评分
        /// </summary>
        public float GenitaliaScore { get; set; }
    }
}
