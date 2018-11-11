using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Finance
{
    public class BuySheep : Expenditure
    {
        public string SheepId { get; set; }

        public string SerialNumber { get; set; }

        public string Source { get; set; }

        public float? Weight { get; set; }
    }
}
