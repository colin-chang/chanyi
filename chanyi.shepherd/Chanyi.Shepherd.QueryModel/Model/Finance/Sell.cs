using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Finance
{
    public abstract class Sell : BaseModelWithPrincipal
    {
        public decimal Price { get; set; }

        /// <summary>
        /// 购买人ID
        /// </summary>
        public string PurchaserId { get; set; }

        public string Purchaser { get; set; }

        public string Department { get; set; }

        public DateTime OperationDate { get; set; }
    }
}
