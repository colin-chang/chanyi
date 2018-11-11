using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.BindingModel
{
    public class BuyInputBind : BaseBindModel
    {
        /// <summary>
        /// LinkId(出入库Id)
        /// </summary>
        //public string Id { get; set; }

        //public string NameId { get; set; }
        public string Name { get; set; }

        public float Amount { get; set; }

        public DateTime  OperationDate { get; set; }
    }
}
