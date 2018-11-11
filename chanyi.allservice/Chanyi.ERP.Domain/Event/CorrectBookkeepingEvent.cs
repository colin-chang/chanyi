using Chanyi.Common.Domain;
using Chanyi.ERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.Domain.Event
{
    public class CorrectBookkeepingEvent : IEvent
    {
        public string OldId { get; set; }

        public string NewId { get; set; }

        public DateTime Time { get; set; }

        public string NewVoucherType { get; set; }

        public string NewVoucherNum { get; set; }

        public string NewAbstract { get; set; }

        public decimal RbkAmount { get; set; }

        public decimal NewAmount { get; set; }

        public BookkeepingDirectionEnum RbkDirection { get; set; }

        public BookkeepingDirectionEnum NewDirection { get; set; }

        public decimal RbkBalance { get; set; }

        public decimal NewBalance { get; set; }

        public string NewCreaterId { get; set; }

        public DateTime CreateTime { get; set; }

        public string AbandonReason { get; set; }

        public BookkeepingStatusEnum OldStatus { get; set; }

        public BookkeepingStatusEnum RbkStatus { get; set; }

        public BookkeepingStatusEnum NewStatus { get; set; }


        public string NewRemark { get; set; }

    }
}
