using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Multiplying
{
    /// <summary>
    /// 分娩
    /// </summary>
    public class Delivery : BaseModelWithPrincipal
    {
        public string SheepId { get; set; }

        public string FemaleNumber { get; set; }

        public DeliveryWayEnum DeliveryWay { get; set; }

        public MidwiferyReasonEnum DeliverReason { get; set; }

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
        public int TotalCount { get; set; }

        public DateTime DeliveryDate { get; set; }

        public DateTime MatingDate { get; set; }

        public bool IsDel { get; set; }
    }
}
