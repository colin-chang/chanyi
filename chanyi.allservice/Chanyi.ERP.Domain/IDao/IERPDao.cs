using Chanyi.Doamin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.Domain.IDao
{
    public interface IERPDao
    {
        /// <summary>
        /// 查询指定ID的账户条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int GetAccountCount(string id);

        /// <summary>
        /// 查询最后一次记账余额
        /// </summary>
        /// <returns></returns>
        decimal GetLastBalance();

        /// <summary>
        /// 查询余额
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        decimal GetBalanceById(string id);

        /// <summary>
        /// 查询现金日记账
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BadBookkeeping GetBadBookkeeping(string id);

        /// <summary>
        /// 查询指定ID的部门条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int GetDepartmentCount(string id);

        /// <summary>
        /// 查询指定ID的职务条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int GetDutyCount(string id);

        /// <summary>
        /// 查询指定ID的用户条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int GetUserCount(string id);

        /// <summary>
        /// 查询User总人数
        /// </summary>
        /// <returns></returns>
        int GetUserTotalCount();

        /// <summary>
        /// 查询指定ID的账户条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int GetBookkeepingCount(string id);

    }
}
