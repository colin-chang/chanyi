using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.HR
{
    public class Employee : BaseModelWithPrincipal
    {
        public string Name { get; set; }

        public GenderEnum Gender { get; set; }

        public string IdNum { get; set; }

        public DateTime EntryDate { get; set; }

        public decimal Salary { get; set; }

        public string SerialNum { get; set; }

        public string DutyName { get; set; }
        public string DutyId { get; set; }

        public EmployeeStatusEnum Status { get; set; }
    }
}
