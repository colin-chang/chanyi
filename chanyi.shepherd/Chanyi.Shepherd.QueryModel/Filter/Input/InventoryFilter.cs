using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Input
{
    public abstract class InventoryFilter : BaseFilter
    {
        public string NameId { get; set; }

        public float? MaxAmount { get; set; }
        public float? MinAmount { get; set; }
    }
}
