using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.ReportForms
{
    /// <summary>
    /// 卖出报表
    /// </summary>
    public class SellReport:BaseFinanceReport
    {
        public decimal SellFeed { get; set; }
        /// <summary>   
        /// 羊粪
        /// </summary>
        public decimal SellManure { get; set; }
        public decimal SellOther { get; set; }
        public decimal SellSheep { get; set; }
        public decimal SellWool { get; set; }

    }
}
