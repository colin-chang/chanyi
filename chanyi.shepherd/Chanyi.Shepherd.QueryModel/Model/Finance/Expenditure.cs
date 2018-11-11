using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Finance
{
    /// <summary>
    /// 资金支出
    /// </summary>
    public abstract class Expenditure : BaseModelWithPrincipal
    {
        public decimal Money { get; set; }

        public DateTime OperationDate { get; set; }
    }
}
