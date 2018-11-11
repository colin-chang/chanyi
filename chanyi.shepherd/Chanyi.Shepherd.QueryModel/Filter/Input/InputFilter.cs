using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Input
{
    public abstract class InputFilter : BaseModelWithPrincipalFilter
    {
        public string NameId { get; set; }
    }
}
