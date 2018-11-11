using Chanyi.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.Domain.Event
{
    public class UpdateAccountEvent:IEvent
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }
    }
}
