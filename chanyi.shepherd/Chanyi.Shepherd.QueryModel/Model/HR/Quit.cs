using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.HR
{
    /// <summary>
    /// 离职
    /// </summary>
    public class Quit : BaseModelWithPrincipal
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        /// <summary>
        /// 离职时间
        /// </summary>
        public DateTime QuitDate { get; set; }
        public string Reason { get; set; }

    }
}
