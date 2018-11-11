using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Input
{
    /// <summary>
    /// 饲料出入库
    /// </summary>
    public class FeedInOut : InOutWarehouse
    {
        public string Area { get; set; }
    }
}
