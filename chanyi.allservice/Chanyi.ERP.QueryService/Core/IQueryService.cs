using Chanyi.ERP.QueryService.Core.Dto;
using Chanyi.ERP.QueryService.Core.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.QueryService.Core
{
    [ServiceContract]
    public interface IQueryService
    {
        /// <summary>
        /// 查询用户账户
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract]
        List<Accounts> GetAccounts(AccountFilter filter);

        /// <summary>
        /// 查询现金日记账
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract]
        List<Bookkeeping> GetBookkeeping(int pageIndex, int pageSize, BookkeepingFilter filter);
        
        /// <summary>
        /// 查询现金日记账条数
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract]
        int GetBookkeepingCount(BookkeepingFilter filter);

        /// <summary>
        /// 查询部门
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract]
        List<Department> GetDepartment(DepartmentFilter filter);

        /// <summary>
        /// 查询职务
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract]
        List<Duty> GetDuty(DutyFilter filter);

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract]
        List<User> GetUser(int pageIndex, int pageSize, UserFilter filter);

        /// <summary>
        /// 查询用户条数
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract]
        int GetUserCount(UserFilter filter);
    }
}
