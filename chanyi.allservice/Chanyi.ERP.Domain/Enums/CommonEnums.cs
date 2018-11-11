using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.Domain.Enums
{
    public enum BookkeepingDirectionEnum
    {
        Borrow,

        /// <summary>
        /// 贷
        /// </summary>
        Loan
    }


    public enum BookkeepingStatusEnum
    {
        Normal,
        Abandon,
        RoolBack,

        /// <summary>
        /// 修正
        /// </summary>
        Amend

    }

    public enum GenderEnum
    {
        UnKnown,
        Men,
        Women
    }

    public enum UserStatus
    {
        OnJob,

        /// <summary>
        /// 离职
        /// </summary>
        Dimission,

        /// <summary>
        /// 休假
        /// </summary>
        OnHoliday
    }

}
