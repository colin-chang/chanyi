using Chanyi.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.Domain.Event
{
    public class DeleteDutyEvent:IEvent
    {
        public string Id { get; set; }
    }
}
