using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Multiplying
{
    /// <summary>
    /// 分娩
    /// </summary>
    public class DeliveryFilter : BaseModelWithPrincipalFilter
    {
        public string SheepId { get; set; }

        public DeliveryWayEnum? DeliveryWay { get; set; }
        /// <summary>
        /// 助产原因
        /// </summary>
        public MidwiferyReasonEnum? DeliverReason { get; set; }
        /// <summary>
        /// 助产原因为其他时的详情
        /// </summary>
        public string DeliverReasonOtherDetail { get; set; }

        /// <summary>
        /// 产活公羔数
        /// </summary>
        public int? LiveMaleCount { get; set; }
        /// <summary>
        /// 产活母羔数
        /// </summary>
        public int? LiveFemaleCount { get; set; }

        /// <summary>
        /// 总产羔数
        /// </summary>
        public int? TotalDeliveryCount { get; set; }

        public DateTime? StartDeliveryDate { get; set; }
        public DateTime? EndDeliveryDate { get; set; }

        public bool IsDel { get { return false; } }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                #region 基本的

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("d.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("d.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }
                if (StartCreateTime != null)
                {
                    list.Add("d.\"CreateTime\">=@StartCreateTime");
                    pms.AddWithValue("StartCreateTime", StartCreateTime);
                }
                if (EndCreateTime != null)
                {
                    list.Add("d.\"CreateTime\"<=@EndCreateTime");
                    pms.AddWithValue("EndCreateTime", EndCreateTime);
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("d.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(PrincipalId))
                {
                    list.Add("d.\"PrincipalId\" = @PrincipalId");
                    pms.AddWithValue("PrincipalId", PrincipalId);
                }
                #endregion

                if (!string.IsNullOrWhiteSpace(SheepId))
                {
                    list.Add("d.\"SheepId\"=@SheepId");
                    pms.AddWithValue("SheepId", SheepId);
                }

                if (DeliveryWay != null)
                {
                    list.Add("d.\"DeliveryWay\"=@DeliveryWay");
                    pms.AddWithValue("DeliveryWay", (int)DeliveryWay);
                }


                if (DeliverReason != null)
                {
                    list.Add("\"MidwiferyReasonEnum\"=@MidwiferyReasonEnum");
                    pms.AddWithValue("MidwiferyReasonEnum", (int)DeliverReason);
                }

                if (!string.IsNullOrWhiteSpace(DeliverReasonOtherDetail))
                {
                    list.Add("\"DeliverReasonOtherDetail\" like @DeliverReasonOtherDetail");
                    pms.AddWithValue("DeliverReasonOtherDetail", DeliverReasonOtherDetail.Wrap("%"));
                }

                if (LiveMaleCount != null)
                {
                    list.Add("\"LiveMaleCount\"=@LiveMaleCount");
                    pms.AddWithValue("LiveMaleCount", LiveMaleCount);
                }

                if (LiveFemaleCount != null)
                {
                    list.Add("\"LiveFemaleCount\"=@LiveFemaleCount");
                    pms.AddWithValue("LiveFemaleCount", LiveFemaleCount);
                }



                if (TotalDeliveryCount != null)
                {
                    list.Add("d.\"TotalCount\"=@TotalDeliveryCount");
                    pms.AddWithValue("TotalDeliveryCount", TotalDeliveryCount);
                }

                if (StartDeliveryDate != null)
                {
                    list.Add("d.\"DeliveryDate\">=@StartDeliveryDate");
                    pms.AddWithValue("StartDeliveryDate", StartDeliveryDate);
                }
                if (EndDeliveryDate != null)
                {
                    list.Add("d.\"DeliveryDate\"<=@EndDeliveryDate");
                    pms.AddWithValue("EndDeliveryDate", EndDeliveryDate);
                }

                list.Add("d.\"IsDel\"=@dIsDel");
                pms.AddWithValue("dIsDel", IsDel);

                list.Add("m.\"IsDel\"=@mIsDel");
                pms.AddWithValue("mIsDel", IsDel);

                return list;
            };
        }
    }
}
