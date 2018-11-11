using Chanyi.Shepherd.QueryModel.Filter;
using Spring.Context;
using Spring.Context.Support;
using Spring.Data.Common;
using Spring.Data.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.BindingFilter
{
    public abstract class BaseBindFilter : BaseFilter
    {
        public string Id { get; set; }
    }
}
