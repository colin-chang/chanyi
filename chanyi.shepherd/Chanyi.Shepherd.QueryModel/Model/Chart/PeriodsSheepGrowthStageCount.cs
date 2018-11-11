using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Chart
{
    /// <summary>
    /// 时间段内，成长阶段羊只统计
    /// </summary>
    public class PeriodsSheepGrowthStageCount
    {
        public GrowthStageEnum GrowthStage { get; set; }

        public long Count { get; set; }

    }
}
