using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public abstract class SellInputFilter:SellFilter
    {
        public string NameId { get; set; }
    }
}
