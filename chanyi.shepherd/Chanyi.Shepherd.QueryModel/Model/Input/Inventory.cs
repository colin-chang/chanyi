using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Input
{
    /// <summary>
    /// 库存
    /// </summary>
    public abstract class Inventory
    {
        public string Id { get; set; }

        public string KindId { get; set; }

        public float Amount { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
