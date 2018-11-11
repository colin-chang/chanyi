using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.Filter.HR
{
    public class QuitFilter : BaseModelWithPrincipalFilter
    {
        public string EmployeeId { get; set; }
        public string Reason { get; set; }
        public DateTime? StartQuitDate { get; set; }
        public DateTime? EndQuitDate { get; set; }
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();


                if (!string.IsNullOrWhiteSpace(PrincipalId))
                {
                    list.Add("q.\"PrincipalId\"=@PrincipalId");
                    pms.AddWithValue("PrincipalId", PrincipalId);
                }


                if (!string.IsNullOrWhiteSpace(EmployeeId))
                {
                    list.Add("\"EmployeeId\"=@EmployeeId");
                    pms.AddWithValue("EmployeeId", EmployeeId);
                }


                if (!string.IsNullOrWhiteSpace(Reason))
                {
                    list.Add("\"Reason\"=@Reason");
                    pms.AddWithValue("Reason", Reason);
                }


                if (StartQuitDate != null)
                {
                    list.Add("\"QuitDate\">=@StartQuitDate");
                    pms.AddWithValue("StartQuitDate", StartQuitDate);
                }
                if (EndQuitDate != null)
                {
                    list.Add("\"QuitDate\"<=@EndQuitDate");
                    pms.AddWithValue("EndQuitDate", EndQuitDate);
                }

                return list;
            };
        }
    }
}
