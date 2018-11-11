using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter
{
    public abstract class BaseModelWithPrincipalFilter : BaseModelFilter
    {
        public string PrincipalId { get; set; }
    }
}
