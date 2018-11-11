using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;


using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Formula
{
    public class FormulaFilter : BaseModelWithPrincipalFilter
    {
        public string Name { get; set; }

        /// <summary>
        /// 适用于
        /// </summary>
        public string ApplyTo { get; set; }

        /// <summary>
        /// 不良反应
        /// </summary>
        public string SideEffect { get; set; }

        public bool? IsEnable { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                #region 基本
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("f.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (StartCreateTime != null)
                {
                    list.Add("f.\"CreateTime\">=@StartCreateTime");
                    pms.AddWithValue("StartCreateTime", StartCreateTime);
                }
                if (EndCreateTime != null)
                {
                    list.Add("f.\"CreateTime\"<=@EndCreateTime");
                    pms.AddWithValue("EndCreateTime", EndCreateTime);
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("f.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(PrincipalId))
                {
                    list.Add("f.\"PrincipalId\" = @PrincipalId");
                    pms.AddWithValue("PrincipalId", PrincipalId);
                }
                #endregion


                if (!string.IsNullOrWhiteSpace(Name))
                {
                    list.Add("f.\"Name\" like @Name");
                    pms.AddWithValue("Name", Name.Wrap("%"));
                }


                if (!string.IsNullOrWhiteSpace(ApplyTo))
                {
                    list.Add("f.\"ApplyTo\" like @ApplyTo");
                    pms.AddWithValue("ApplyTo", ApplyTo.Wrap("%"));
                }


                if (!string.IsNullOrWhiteSpace(SideEffect))
                {
                    list.Add("f.\"SideEffect\" like @SideEffect");
                    pms.AddWithValue("SideEffect", SideEffect.Wrap("%"));
                }

                if (IsEnable != null)
                {
                    list.Add("f.\"IsEnable\"=@IsEnable");
                    pms.AddWithValue("IsEnable", IsEnable);
                }

                return list;
            };
        }
    }
}
