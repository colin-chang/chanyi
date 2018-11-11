using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter
{
    public abstract class BaseModelFilter : BaseFilter
    {
        public string Id { get; set; }

        public string OperatorId { get; set; }

        public DateTime? StartCreateTime { get; set; }

        public DateTime? EndCreateTime { get; set; }

        public string Remark { get; set; }
    }
}
