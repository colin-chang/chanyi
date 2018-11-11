using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public abstract class SellFilter : BaseModelWithPrincipalFilter
    {
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// 购买人ID
        /// </summary>
        public string PurchaserId { get; set; }

        public DateTime? StartOperationDate { get; set; }
        public DateTime? EndOperationDate { get; set; }

        protected List<string> GetSellSqlWhere(out IDbParameters pms)
        {
            List<string> list = new List<string>();
            pms = Template.CreateDbParameters();

            #region 基本的
            if (!string.IsNullOrWhiteSpace(Id))
            {
                list.Add("s.\"Id\"=@Id");
                pms.AddWithValue("Id", Id);
            }
            if (!string.IsNullOrWhiteSpace(OperatorId))
            {
                list.Add("s.\"OperatorId\"=@OperatorId");
                pms.AddWithValue("OperatorId", OperatorId);
            }
            if (StartCreateTime != null)
            {
                list.Add("s.\"CreateTime\">=@StartCreateTime");
                pms.AddWithValue("StartCreateTime", StartCreateTime);
            }
            if (EndCreateTime != null)
            {
                list.Add("s.\"CreateTime\"<=@EndCreateTime");
                pms.AddWithValue("EndCreateTime", EndCreateTime);
            }
            if (!string.IsNullOrWhiteSpace(Remark))
            {
                list.Add("s.\"Remark\" like @Remark");
                pms.AddWithValue("Remark", Remark.Wrap("%"));
            }

            if (!string.IsNullOrWhiteSpace(PrincipalId))
            {
                list.Add("s.\"PrincipalId\" = @PrincipalId");
                pms.AddWithValue("PrincipalId", PrincipalId);
            }
            #endregion


            if (MaxPrice != null)
            {
                list.Add("s.\"Price\"<=cast(@MaxPrice as money)");
                pms.AddWithValue("MaxPrice", MaxPrice);
            }
            if (MinPrice != null)
            {
                list.Add("s.\"Price\">=cast(@MinPrice as money)");
                pms.AddWithValue("MinPrice", MinPrice);
            }


            if (!string.IsNullOrWhiteSpace(PurchaserId))
            {
                list.Add("s.\"PurchaserId\"=@PurchaserId");
                pms.AddWithValue("PurchaserId", PurchaserId);
            }


            if (StartOperationDate != null)
            {
                list.Add("s.\"OperationDate\">=@StartOperationDate");
                pms.AddWithValue("StartOperationDate", StartOperationDate);
            }
            if (EndOperationDate != null)
            {
                list.Add("s.\"OperationDate\"<=@EndOperationDate");
                pms.AddWithValue("EndOperationDate", EndOperationDate);
            }

            return list;
        }
    }
}
