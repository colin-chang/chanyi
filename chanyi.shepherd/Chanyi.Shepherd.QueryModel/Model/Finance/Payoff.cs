using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chanyi.Shepherd.QueryModel.Filter;
using Spring.Data.Common;

namespace Chanyi.Shepherd.QueryModel.Model.Finance
{
    /// <summary>
    /// 员工工资
    /// </summary>
    public class Payoff :Expenditure
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }

    }
}
