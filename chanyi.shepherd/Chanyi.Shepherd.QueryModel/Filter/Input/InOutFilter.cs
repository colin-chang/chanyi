using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Input
{
    public abstract class InOutFilter : BaseModelWithPrincipalFilter
    {
        //public string KindId { get; set; }

        public float? MaxAmount { get; set; }
        public float? MinAmount { get; set; }

        public string NameId { get; set; }

        public InOutWarehouseDirectionEnum? Direction { get; set; }
        /// <summary>
        /// 出库去向
        /// </summary>
        public OutWarehouseDispositonEnum? Dispositon { get; set; }
        public DateTime? StartOperationDate { get; set; }
        public DateTime? EndOperationDate { get; set; }
    }
}
