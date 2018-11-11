
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.Common;

using Chanyi.Utility.Common;
using Chanyi.Shepherd.QueryModel.Filter;

namespace Chanyi.Shepherd.QueryModel.Filter.DiseaseControl
{
    /// <summary>
    /// 疾病治疗
    /// </summary>
    public class TreatmentFilter : BaseModelWithPrincipalFilter
    {
        public string SheepId { get; set; }
        /// <summary>
        /// 症状
        /// </summary>
        public string Symptom { get; set; }

        /// <summary>
        /// 疾病开始时间
        /// </summary>
        public DateTime? StartStartDate { get; set; }
        public DateTime? EndStartDate { get; set; }

        /// <summary>
        /// 疾病（兽医诊断结果）
        /// </summary>
        public string Disease { get; set; }
        /// <summary>
        /// 用药详情
        /// </summary>
        public string TreatmentPlan { get; set; }

        /// <summary>
        /// 治疗结果
        /// </summary>
        public string Effect { get; set; }
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                #region 基本

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("t.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("t.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }
                if (StartCreateTime != null)
                {
                    list.Add("t.\"CreateTime\">=@StartCreateTime");
                    pms.AddWithValue("StartCreateTime", StartCreateTime);
                }
                if (EndCreateTime != null)
                {
                    list.Add("t.\"CreateTime\"<=@EndCreateTime");
                    pms.AddWithValue("EndCreateTime", EndCreateTime);
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("t.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(PrincipalId))
                {
                    list.Add("t.\"PrincipalId\" = @PrincipalId");
                    pms.AddWithValue("PrincipalId", PrincipalId);
                }

                #endregion

                if (!string.IsNullOrWhiteSpace(SheepId))
                {
                    list.Add("\"SheepId\"=@SheepId");
                    pms.AddWithValue("SheepId", SheepId);
                }

                if (!string.IsNullOrWhiteSpace(Symptom))
                {
                    list.Add("\"Symptom\" like @Symptom");
                    pms.AddWithValue("Symptom", Symptom.Wrap("%"));
                }

                if (StartStartDate != null)
                {
                    list.Add("\"StartDate\">=@StartStartDate");
                    pms.AddWithValue("StartStartDate", StartStartDate);
                }
                if (EndStartDate != null)
                {
                    list.Add("\"StartDate\"<=@EndStartDate");
                    pms.AddWithValue("EndStartDate", EndStartDate);
                }


                if (!string.IsNullOrWhiteSpace(Disease))
                {
                    list.Add("\"Disease\" like @Disease");
                    pms.AddWithValue("Disease", Disease.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(TreatmentPlan))
                {
                    list.Add("\"TreatmentPlan\" like @TreatmentPlan");
                    pms.AddWithValue("TreatmentPlan", TreatmentPlan.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(Effect))
                {
                    list.Add("\"Effect\" like @Effect");
                    pms.AddWithValue("Effect", Effect.Wrap("%"));
                }


                return list;
            };
        }
    }
}
