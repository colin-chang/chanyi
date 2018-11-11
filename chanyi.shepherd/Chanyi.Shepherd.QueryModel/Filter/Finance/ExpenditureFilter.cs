using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public abstract class ExpenditureFilter : BaseModelWithPrincipalFilter
    {
        public decimal? MaxMoney { get; set; }
        public decimal? MinMoney { get; set; }

        public DateTime? StartOperationDate { get; set; }
        public DateTime? EndOperationDate { get; set; }

        protected List<string> GetBaseSqlWhere(out IDbParameters pms)
        {
            List<string> list = new List<string>();
            pms = Template.CreateDbParameters();

            #region 基本的
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


            if (MaxMoney != null)
            {
                list.Add("e.\"Money\"<=cast(@MaxMoney as money)");
                pms.AddWithValue("MaxMoney", MaxMoney);
            }
            if (MinMoney != null)
            {
                list.Add("e.\"Money\">=cast(@MinMoney as money)");
                pms.AddWithValue("MinMoney", MinMoney);
            }

            if (StartOperationDate != null)
            {
                list.Add("e.\"OperationDate\">=@StartOperationDate");
                pms.AddWithValue("StartOperationDate", StartOperationDate);
            }
            if (EndOperationDate != null)
            {
                list.Add("e.\"OperationDate\"<=@EndOperationDate");
                pms.AddWithValue("EndOperationDate", EndOperationDate);
            }

            return list;
        }

    }
}
