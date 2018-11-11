using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Finance
{
    /// <summary>
    /// 出售投入品
    /// </summary>
    public class SellInput : Sell
    {
        public string Name { get; set; }
        public string LinkId { get; set; }

        public float Amout { get; set; }
    }
}
