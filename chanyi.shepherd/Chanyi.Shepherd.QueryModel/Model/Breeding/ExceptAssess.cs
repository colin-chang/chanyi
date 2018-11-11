using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Breeding
{
    /// <summary>
    /// 排除羊只
    /// </summary>
    public class ExceptAssess : BaseModelWithPrincipal
    {
        public string SerialNumber { get; set; }
        public string Reason { get; set; }
    }
}
