using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.System
{
    /// <summary>
    /// 职务
    /// </summary>
    public class Duty : BaseModelWithPrincipal
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
