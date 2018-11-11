using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.ReportForms
{

    public class FeedSheepReport
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Area { get; set; }
        public float TotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
