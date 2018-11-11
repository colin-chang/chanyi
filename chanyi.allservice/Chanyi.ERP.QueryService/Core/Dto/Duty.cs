using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chanyi.ERP.QueryService.Core.Dto
{
    [DataContract]
    public class Duty
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Desc { get; set; }

        [DataMember]
        public string DepartmentName { get; set; }

        [DataMember]
        public bool IsEnabled { get; set; }

        [DataMember]
        public string CreaterName { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }
    }
}
