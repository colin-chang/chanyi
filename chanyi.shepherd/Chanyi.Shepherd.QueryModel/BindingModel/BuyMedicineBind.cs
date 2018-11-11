using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.BindingModel
{
    public class BuyMedicineBind : BuyInputBind
    {
        public string ManufacturerId { get; set; }
        public string Manufacturer { get; set; }
    }
}
