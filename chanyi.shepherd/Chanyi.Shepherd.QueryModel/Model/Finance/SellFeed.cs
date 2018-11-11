using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Finance
{
    /// <summary>
    /// 卖饲料
    /// </summary>
    public class SellFeed:SellInput
    {
        public string Type { get; set; }
        public string Area { get; set; }
    }
}
