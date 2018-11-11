using Chanyi.Common.Domain;
using Chanyi.ERP.Domain.Enums;
using Chanyi.ERP.Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Doamin
{
    public class Bookkeeping
    {
        public string Id { get; private set; }

        public DateTime Time { get; private set; }

        public string VoucherType { get; private set; }

        public string VoucherNum { get; private set; }

        public string Abstract { get; private set; }

        public decimal Amount { get; private set; }

        public int Direction { get; private set; }

        public decimal Balance { get; private set; }

        public string CreaterId { get; private set; }

        public DateTime CreateTime { get; private set; }

        public string ReferenceId { get; private set; }

        public string AbandonReason { get; private set; }

        public int Status { get; private set; }

        public string Remark { get; private set; }

        public string Create(DateTime time, string voucherTypeId, string voucherNum, string abst, decimal amount, int direction, decimal balance, string remark, string createrId)
        {
            string id = Guid.NewGuid().ToString();

            DomainHelper.Publish(new CreateBookkeepingEvent { Id = id, Abstract = abst, Amount = amount, Balance = balance, CreaterId = createrId, CreateTime = DateTime.Now, Direction = direction, Remark = remark, Status = (int)BookkeepingStatusEnum.Normal, Time = time, VoucherNum = voucherNum, VoucherType = voucherTypeId });

            return id;
        }

        public void Correct(string newRemark, string oldId, string newCreaterId, string newVoucherType, decimal rbkBalance, BookkeepingDirectionEnum rbkDirection, string abandonReason, string newAbstract, decimal newAmount, decimal newBalance, BookkeepingDirectionEnum newDirection, string newVoucherNum, decimal rbkAmount, DateTime time)
        {
            string rbkId = Guid.NewGuid().ToString();
            string newId = Guid.NewGuid().ToString();

            DomainHelper.Publish(new CorrectBookkeepingEvent { NewRemark = newRemark, NewStatus = BookkeepingStatusEnum.Normal, OldId = oldId, NewCreaterId = newCreaterId, NewVoucherType = newVoucherType, NewId = newId, RbkStatus = BookkeepingStatusEnum.RoolBack, RbkBalance = rbkBalance, RbkDirection = rbkDirection, OldStatus = BookkeepingStatusEnum.Abandon, AbandonReason = abandonReason, CreateTime = DateTime.Now, NewAbstract = newAbstract, NewAmount = newAmount, NewBalance = newBalance, NewDirection = newDirection, NewVoucherNum = newVoucherNum, RbkAmount = rbkAmount, Time = time });
        }
    }

    public class BadBookkeeping
    {
        public decimal Amount { get; set; }

        public BookkeepingDirectionEnum Direction { get; set; }

        public BookkeepingStatusEnum Status { get; set; }
    }
}
