using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Input
{
    public class Medicine : Input
    {
        /// <summary>
        /// 生产商
        /// </summary>
        public string ManufacturerId { get; set; }
        public string Manufacturer { get; set; }
        public string TypeId { get; set; }
        public string Type { get; set; }
        public string Department { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Unit { get; set; }
    }
}
