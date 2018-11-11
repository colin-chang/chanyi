using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.System
{
    public class MedicineCritical : Settings
    {
        public string KindId { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string MedicineType { get; set; }
    }
}
