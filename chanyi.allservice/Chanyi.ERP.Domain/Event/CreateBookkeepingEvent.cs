using Chanyi.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.Domain.Event
{
    public class CreateBookkeepingEvent : IEvent
    {
        public string Id { get; set; }

        public DateTime Time { get; set; }

        public string VoucherType { get; set; }

        public string VoucherNum { get; set; }

        public string Abstract { get; set; }

        public decimal Amount { get; set; }

        public int Direction { get; set; }

        public decimal Balance { get; set; }

        public string CreaterId { get; set; }

        public DateTime CreateTime { get; set; }

        public int Status { get; set; }

        public string Remark { get; set; }
    }
}
