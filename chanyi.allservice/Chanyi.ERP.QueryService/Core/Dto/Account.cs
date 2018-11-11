using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.QueryService.Core.Dto
{
    [DataContract]
    public class Accounts
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Account { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string CreaterName { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }
    }
}
