using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Finance
{
    public class BuyMedicine : BuyInput
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }

        public string Department { get; set; }
        public string Unit { get; set; }
    }
}
