using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.DiseaseControl
{
    /// <summary>
    /// 疾病治疗
    /// </summary>
    public class Treatment : BaseModelWithPrincipal
    {
        public string SheepId { get; set; }

        public string SerialNumber { get; set; }

        /// <summary>
        /// 症状
        /// </summary>
        public string Symptom { get; set; }
        /// <summary>
        /// 疾病开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 疾病（兽医诊断结果）
        /// </summary>
        public string Disease { get; set; }
        /// <summary>
        /// 用药详情
        /// </summary>
        public string TreatmentPlan { get; set; }
        /// <summary>
        /// 治疗时长
        /// </summary>
        public int TreatmentDays { get; set; }
        /// <summary>
        /// 治疗结果
        /// </summary>
        public string Effect { get; set; }
    }
}
