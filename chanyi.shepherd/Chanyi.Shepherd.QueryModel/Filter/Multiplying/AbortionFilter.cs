using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Multiplying
{
    public class AbortionFilter : BaseModelWithPrincipalFilter
    {
        public string SheepId { get; set; }

        public string Reason { get; set; }

        public string Dispose { get; set; }

        public DateTime? StartAbortionDate { get; set; }
        public DateTime? EndAbortionDate { get; set; }
        public bool IsDel { get { return false; } }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

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



                if (!string.IsNullOrWhiteSpace(SheepId))
                {
                    list.Add("a.\"SheepId\"=@SheepId");
                    pms.AddWithValue("SheepId", SheepId);
                }


                if (!string.IsNullOrWhiteSpace(Reason))
                {
                    list.Add("a.\"Reason\" like @Reason");
                    pms.AddWithValue("Reason", Reason.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(Dispose))
                {
                    list.Add("a.\"Dispose\" like @Dispose");
                    pms.AddWithValue("Dispose", Dispose.Wrap("%"));
                }

                if (StartAbortionDate != null)
                {
                    list.Add("a.\"AbortionDate\">=@StartAbortionDate");
                    pms.AddWithValue("StartAbortionDate", StartAbortionDate);
                }
                if (EndAbortionDate != null)
                {
                    list.Add("a.\"AbortionDate\"<=@EndAbortionDate");
                    pms.AddWithValue("EndAbortionDate", EndAbortionDate);
                }

                list.Add("m.\"IsDel\"=@mIsDel");
                pms.AddWithValue("mIsDel", IsDel);

                list.Add("a.\"IsDel\"=@aIsDel");
                pms.AddWithValue("aIsDel", IsDel);


                return list;
            };
        }
    }
}
