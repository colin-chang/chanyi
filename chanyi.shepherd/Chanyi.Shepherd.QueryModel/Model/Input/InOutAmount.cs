using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Input
{
    /// <summary>
    /// 出入库方向与数量
    /// </summary>
    public class InOutAmount
    {
        public InOutWarehouseDirectionEnum Direction { get; set; }
        public float Amount { get; set; }
    }
}
