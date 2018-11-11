using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.DiseaseControl
{
    /// <summary>
    /// 疾病防疫计划
    /// </summary>
    public class AntiepidemicPlan : BaseModelWithPrincipal
    {
        /// <summary>
        /// 计划名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 疫苗
        /// </summary>
        public string Vaccine { get; set; }
        /// <summary>
        /// 计划执行时间
        /// </summary>
        public DateTime PlanExecuteDate { get; set; }
        /// <summary>
        /// 计划防疫羊群
        /// </summary>
        public string SheepFlock { get; set; }

        /// <summary>
        /// 是否执行了
        /// </summary>
        public bool IsExcuted { get; set; }

        /// <summary>
        /// 执行防疫计划的Id
        /// </summary>
        public string ExcuteAntiepidemicId { get; set; }
    }
}
