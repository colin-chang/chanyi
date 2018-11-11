
using Chanyi.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.Domain.Event
{
    public class CreateDepartmentEvent:IEvent
    {

        public string Id { get;  set; }

        public string Name { get;  set; }

        public string Desc { get;  set; }

        public bool IsEnabled { get;  set; }

        public string CreaterId { get;  set; }

        public DateTime CreateTime { get;  set; }
    }
}
