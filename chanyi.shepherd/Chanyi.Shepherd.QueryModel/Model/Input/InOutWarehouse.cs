using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Input
{
    /// <summary>
    /// 出入库日志
    /// </summary>
    public abstract class InOutWarehouse : BaseModelWithPrincipal
    {
        /// <summary>
        /// 具体一种投入品ID
        /// </summary>
        public string KindId { get; set; }

        public float Amount { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public InOutWarehouseDirectionEnum Direction { get; set; }
        /// <summary>
        /// 出库去向
        /// </summary>
        public OutWarehouseDispositonEnum Dispositon { get; set; }

        public DateTime OperationDate { get; set; }
    }
}
