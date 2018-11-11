using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.System
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Permission : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }

    }
}
