using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.AddModel.Finance
{
    /// <summary>
    /// 出售投入品
    /// </summary>
    public class AddSellInput
    {
        /// <summary>
        /// 出入库Id
        /// </summary>
        public string LinkId { get; set; }

        public decimal Price { get; set; }
        /// <summary>
        /// 购买人
        /// </summary>
        public string PurchaserId { get; set; }

        public DateTime operationDate { get; set; }

        public string PrincipalId { get; set; }
    }
}
