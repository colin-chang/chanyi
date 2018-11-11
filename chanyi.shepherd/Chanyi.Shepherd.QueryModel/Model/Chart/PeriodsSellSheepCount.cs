using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Chart
{
    /// <summary>
    /// 一定时间段内出售羊只的数量
    /// </summary>
    public class PeriodsSellSheepCount
    {
        public string Month { get; set; }
        public long Count { get; set; }
    }
}
