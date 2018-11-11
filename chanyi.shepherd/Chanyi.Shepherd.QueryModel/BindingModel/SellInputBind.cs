using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.BindingModel
{
    public class SellInputBind : BaseBindModel
    {
        public string LinkId { get { return this.Id; } }

        public string Name { get; set; }

        public float Amount { get; set; }

        public DateTime OperationDate { get; set; }
    }
}
