using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

namespace Chanyi.Shepherd.QueryModel.BindingFilter
{
    public abstract class SicknessBindFilter : BaseBindFilter
    {
        public string TypeId { get; set; }

        public string Name { get; set; }
    }
}