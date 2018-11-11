using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Input
{
    /// <summary>
    /// 饲料羊只出库
    /// </summary>
    public class FeedSheepOutWarehouse
    {
        public string Id { get; set; }
        public string ShepfoldId { get; set; }
        public string SheepId { get; set; }
        public string GrowthStage { get; set; }
        public string KindId { get; set; }
        public string Amount { get; set; }
        public string Price { get; set; }
        public string OutWarehouseId { get; set; }


    }
}
