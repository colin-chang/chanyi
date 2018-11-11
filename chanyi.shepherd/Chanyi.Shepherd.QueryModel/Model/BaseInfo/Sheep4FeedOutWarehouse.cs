using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.BaseInfo
{
    /// <summary>
    /// 羊只食用饲料专用
    /// </summary>
    public class Sheep4FeedOutWarehouse
    {
        public string Id { get; set; }
        public GrowthStageEnum GrowthStage { get; set; }
        public string SheepfoldId { get; set; }
    }
}
