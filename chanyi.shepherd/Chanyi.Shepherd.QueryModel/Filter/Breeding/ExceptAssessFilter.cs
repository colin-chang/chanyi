using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.Filter.Breeding
{
    public class ExceptAssessFilter : BaseModelWithPrincipalFilter
    {
        public string SheepId { get; set; }
        public string Reason { get; set; }

        public bool? IsDel { get; set; }


        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
             {
                 List<string> list = new List<string>();
                 pms = Template.CreateDbParameters();

                 if (!string.IsNullOrWhiteSpace(Id))
                 {
                     list.Add("ex.\"Id\"=@Id");
                     pms.AddWithValue("Id", Id);
                 }
                 if (!string.IsNullOrWhiteSpace(OperatorId))
                 {
                     list.Add("ex.\"OperatorId\"=@OperatorId");
                     pms.AddWithValue("OperatorId", OperatorId);
                 }
                 if (StartCreateTime != null)
                 {
                     list.Add("ex.\"CreateTime\">=@StartCreateTime");
                     pms.AddWithValue("StartCreateTime", StartCreateTime);
                 }
                 if (EndCreateTime != null)
                 {
                     list.Add("ex.\"CreateTime\"<=@EndCreateTime");
                     pms.AddWithValue("EndCreateTime", EndCreateTime);
                 }
                 if (!string.IsNullOrWhiteSpace(Remark))
                 {
                     list.Add("ex.\"Remark\" like @Remark");
                     pms.AddWithValue("Remark", Remark.Wrap("%"));
                 }

                 if (!string.IsNullOrWhiteSpace(PrincipalId))
                 {
                     list.Add("ex.\"PrincipalId\" = @PrincipalId");
                     pms.AddWithValue("PrincipalId", PrincipalId);
                 }


                 if (!string.IsNullOrWhiteSpace(SheepId))
                 {
                     list.Add("ex.\"SheepId\"=@SheepId");
                     pms.AddWithValue("SheepId", SheepId);
                 }

                 if (!string.IsNullOrWhiteSpace(Reason))
                 {
                     list.Add("\"Reason\" like @Reason");
                     pms.AddWithValue("Reason", Reason.Wrap("%"));
                 }


                 if (IsDel != null)
                 {
                     list.Add("\"IsDel\"=@IsDel");
                     pms.AddWithValue("IsDel", false);
                 }

                 return list;
             };
        }
    }
}
