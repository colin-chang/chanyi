using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.ReportForms
{
    /// <summary>
    /// 花费报表
    /// </summary>
    public class BuyReport:BaseFinanceReport
    {
        public decimal ElectricCharge { get; set; }
        public decimal WaterRate { get; set; }
        public decimal Payoff { get; set; }
        public decimal BuyFeed { get; set; }
        public decimal BuyMedicine { get; set; }
        public decimal BuyOther { get; set; }
        public decimal BuySheep { get; set; }
    }
}
