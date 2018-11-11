using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Assist
{
    public abstract class SicknessFilter : BaseModelWithPrincipalFilter
    {
        public string TypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}