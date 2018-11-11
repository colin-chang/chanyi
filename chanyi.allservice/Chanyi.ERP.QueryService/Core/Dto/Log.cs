using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Chanyi.ERP.QueryService.Core.Dto
{
    [DataContract]
    public class Log
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Operation { get; set; }

        [DataMember]
        public string OperatorName { get; set; }

        [DataMember]
        public DateTime Time { get; set; }

    }
}
