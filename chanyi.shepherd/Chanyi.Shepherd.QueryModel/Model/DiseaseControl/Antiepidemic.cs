using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.DiseaseControl
{
    /// <summary>
    /// 疾病实施
    /// </summary>
    public class Antiepidemic : BaseModelWithPrincipal
    {
        /// <summary>
        /// 防疫名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 疫苗
        /// </summary>
        public string Vaccine { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecuteDate { get; set; }
        /// <summary>
        /// 防疫效果
        /// </summary>
        public string Effect { get; set; }
        /// <summary>
        /// 计划防疫羊群
        /// </summary>
        public string SheepFlock { get; set; }
    }
}
