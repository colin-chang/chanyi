using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Input
{
    /// <summary>
    /// 饲料表
    /// </summary>
    public class Feed : Input
    {
        public string TypeId { get; set; }
        public string AreaId { get; set; }

        public string Type { get; set; }
        public string Area { get; set; }

        public string Description { get; set; }

        public bool IsEditable { get; set; }
    }
}
