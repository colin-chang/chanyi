using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chanyi.Shepherd.QueryModel.Model.System;

namespace Chanyi.Shepherd.QueryModel.Model.Assist
{
    /// <summary>
    /// 疾病
    /// </summary>
    public class Disease : Sickness
    {
        public string SymptomId { get; set; }

    }
}
