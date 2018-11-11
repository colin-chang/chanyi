using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.ReportForms
{
    public class FinanceReport : BaseFinanceReport
    {
        //收入
        public decimal SellFeed { get; set; }
        /// <summary>   
        /// 羊粪
        /// </summary>
        public decimal SellManure { get; set; }
        public decimal SellOther { get; set; }
        public decimal SellSheep { get; set; }
        public decimal SellWool { get; set; }


        //支出
        public decimal ElectricCharge { get; set; }
        public decimal WaterRate { get; set; }
        public decimal Payoff { get; set; }
        public decimal BuyFeed { get; set; }
        public decimal BuyMedicine { get; set; }
        public decimal BuyOther { get; set; }
        public decimal BuySheep { get; set; }
    }
}
