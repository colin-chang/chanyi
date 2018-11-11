using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Input
{
    /// <summary>
    /// 药品出入库
    /// </summary>
    public class MedicineInOut : InOutWarehouse
    {
        public string Manufacturer { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Type { get; set; }
    }
}
