
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
    /// 疾病防疫计划
    /// </summary>
    public class AntiepidemicPlanFilter : BaseModelWithPrincipalFilter
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
        public DateTime? StartPlanExecuteDate { get; set; }
        public DateTime? EndPlanExecuteDate { get; set; }
        /// <summary>
        /// 计划防疫羊群
        /// </summary>
        public string SheepFlock { get; set; }

        /// <summary>
        /// 是否执行了
        /// </summary>
        public bool? IsExcuted { get; set; }
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                #region 基本

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("a.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("a.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }
                if (StartCreateTime != null)
                {
                    list.Add("a.\"CreateTime\">=@StartCreateTime");
                    pms.AddWithValue("StartCreateTime", StartCreateTime);
                }
                if (EndCreateTime != null)
                {
                    list.Add("a.\"CreateTime\"<=@EndCreateTime");
                    pms.AddWithValue("EndCreateTime", EndCreateTime);
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("a.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(PrincipalId))
                {
                    list.Add("a.\"PrincipalId\" = @PrincipalId");
                    pms.AddWithValue("PrincipalId", PrincipalId);
                }

                #endregion

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    list.Add("a.\"Name\" like @Name");
                    pms.AddWithValue("Name", Name.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(Vaccine))
                {
                    list.Add("\"Vaccine\" like @Vaccine");
                    pms.AddWithValue("Vaccine", Vaccine.Wrap("%"));
                }

                if (StartPlanExecuteDate != null)
                {
                    list.Add("\"PlanExecuteDate\">=@StartPlanExecuteDate");
                    pms.AddWithValue("StartPlanExecuteDate", StartPlanExecuteDate);
                }
                if (EndPlanExecuteDate != null)
                {
                    list.Add("\"PlanExecuteDate\"<=@EndPlanExecuteDate");
                    pms.AddWithValue("EndPlanExecuteDate", EndPlanExecuteDate);
                }

                if (!string.IsNullOrWhiteSpace(SheepFlock))
                {
                    list.Add("\"SheepFlock\" like @SheepFlock");
                    pms.AddWithValue("SheepFlock", SheepFlock.Wrap("%"));
                }

                if (IsExcuted != null)
                {
                    list.Add("\"IsExcuted\"=@IsExcuted");
                    pms.AddWithValue("IsExcuted", IsExcuted);
                }

                return list;
            };
        }
    }
}
