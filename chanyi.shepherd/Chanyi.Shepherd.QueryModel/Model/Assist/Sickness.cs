using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Assist
{
    /// <summary>
    /// 疾病、症状
    /// </summary>
    public abstract class Sickness : BaseModelWithPrincipal
    {
        public string Name { get; set; }

        public string TypeId { get; set; }

        public string Description { get; set; }
    }
}
