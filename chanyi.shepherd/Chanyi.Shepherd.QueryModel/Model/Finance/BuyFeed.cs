using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Finance
{
    public class BuyFeed : BuyInput
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Area { get; set; }
    }
}
