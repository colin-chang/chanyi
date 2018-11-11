using Chanyi.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.Domain.Event
{
    public class UpdateUserEvent : IEvent
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string RealName { get; set; }

        public string Password { get; set; }

        public string IdNum { get; set; }

        public string PhoneNum { get; set; }

        public int Gender { get; set; }

        public string Address { get; set; }

        public string Degree { get; set; }

        public DateTime EntryTime { get; set; }

        public string NationId { get; set; }

        public DateTime Birthday { get; set; }

        public string EmployeeNum { get; set; }

        public int Status { get; set; }
    }
}
