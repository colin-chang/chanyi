using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.HR
{
    public class EmployeeFilter : BaseModelWithPrincipalFilter
    {
        public string Name { get; set; }

        public GenderEnum? Gender { get; set; }

        public string IdNum { get; set; }

        public DateTime? StartEntryDate { get; set; }
        public DateTime? EndEntryDate { get; set; }

        public decimal? MaxSalary { get; set; }
        public decimal? MinSalary { get; set; }

        public string SerialNum { get; set; }

        public string DutyId { get; set; }

        //public string DutyName { get; set; }

        public EmployeeStatusEnum? Status { get; set; }


        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                #region 基本信息

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("e.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("e.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }
                if (StartCreateTime != null)
                {
                    list.Add("e.\"CreateTime\">=@StartCreateTime");
                    pms.AddWithValue("StartCreateTime", StartCreateTime);
                }
                if (EndCreateTime != null)
                {
                    list.Add("e.\"CreateTime\"<=@EndCreateTime");
                    pms.AddWithValue("EndCreateTime", EndCreateTime);
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("e.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(PrincipalId))
                {
                    list.Add("e.\"PrincipalId\" = @PrincipalId");
                    pms.AddWithValue("PrincipalId", PrincipalId);
                }

                #endregion

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    list.Add("e.\"Name\" like @Name");
                    pms.AddWithValue("Name", Name.Wrap("%"));
                }

                if (Gender != null)
                {
                    list.Add("e.\"Gender\"=@Gender");
                    pms.AddWithValue("Gender", (int)Gender);
                }

                if (!string.IsNullOrWhiteSpace(IdNum))
                {
                    list.Add("e.\"IdNum\" like @IdNum");
                    pms.AddWithValue("IdNum", IdNum.Wrap("%"));
                }

                if (StartEntryDate != null)
                {
                    list.Add("e.\"EntryDate\">=@StartEntryDate");
                    pms.AddWithValue("StartEntryDate", StartEntryDate);
                }
                if (EndEntryDate != null)
                {
                    list.Add("e.\"EntryDate\"<=@EndEntryDate");
                    pms.AddWithValue("EndEntryDate", EndEntryDate);
                }

                if (MaxSalary != null)
                {
                    list.Add("e.\"Salary\"<=cast(@MaxSalary as money)");
                    pms.AddWithValue("MaxSalary", MaxSalary.ToString());
                }
                if (MinSalary != null)
                {
                    list.Add("e.\"Salary\">=cast(@MinSalary as money)");
                    pms.AddWithValue("MinSalary", MinSalary.ToString());
                }

                if (!string.IsNullOrWhiteSpace(SerialNum))
                {
                    list.Add("e.\"SerialNum\" like @SerialNum");
                    pms.AddWithValue("SerialNum", SerialNum.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(DutyId))
                {
                    list.Add("e.\"DutyId\"=@DutyId");
                    pms.AddWithValue("DutyId", DutyId);
                }

                if (Status != null)
                {
                    list.Add("e.\"Status\"=@Status");
                    pms.AddWithValue("Status", (int)Status);
                }

                return list;
            };
        }
    }
}
