using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chanyi.ERP.QueryService.Core.Dto
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string RealName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string IdNum { get; set; }

        [DataMember]
        public string PhoneNum { get; set; }

        [DataMember]
        public int Gender { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Degree { get; set; }

        [DataMember]
        public DateTime EntryTime { get; set; }

        [DataMember]
        public string NationId { get; set; }

        [DataMember]
        public DateTime? Birthday { get; set; }

        [DataMember]
        public string EmployeeNum { get; set; }

        [DataMember]
        public string CreaterName { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }
    }
}
