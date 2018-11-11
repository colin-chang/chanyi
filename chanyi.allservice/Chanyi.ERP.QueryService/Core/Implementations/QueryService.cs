using Chanyi.ERP.QueryService.Core.Dto;
using Chanyi.ERP.QueryService.Core.Filter;
using Chanyi.ERP.QueryService.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Chanyi.ERP.QueryService.Core.Implementations
{
    [ServiceBehavior(UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class QueryService : IQueryService
    {
        public SqlHelper MySqlHelper { get; set; }

        #region 抽象查询模板

        #region 分页模板

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <typeparam name="TResult">分页数据类型</typeparam>
        /// <typeparam name="TFilter">过滤器类型</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="querySql">查询Sql语句</param>
        /// <param name="countSql">统计Sql语句</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="filter">过滤器</param>
        /// <returns></returns>
        List<TResult> GetPagedData1<TResult, TFilter>(int pageIndex, int pageSize, string countSql, string querySql, IQueryFilter filter)
            where TResult : class,new()
            where TFilter : IQueryFilter, new()
        {
            List<TResult> list = new List<TResult>();

            pageSize = pageSize < 1 ? 20 : pageSize;
            int totalPages = (int)Math.Ceiling(GetDataCount<TFilter>(countSql, filter) * 1.0 / pageSize);
            totalPages = totalPages < 1 ? 1 : totalPages;
            pageIndex = pageIndex < 1 ? 1 : (pageIndex > totalPages ? totalPages : pageIndex);

            MySqlParameter[] paras;
            string where = (filter ?? new TFilter()).ToSqlWhere(out paras);
            //string sql = string.Format("{0} where {1}>=(select {2} {3}{4} order by {5} desc limit {6},1) limit {7}", querySql, orderBy, orderBy, querySql.Substring(querySql.ToLower().IndexOf("from")), where, orderBy, (pageIndex - 1) * pageSize, pageSize);

            string sql = string.Format("{0}  limit {1},{2}", querySql, (pageIndex - 1) * pageSize, pageSize);


            list = MySqlHelper.ExecuteReader<TResult>(sql, paras);

            return list;
        }

        /// <summary>
        /// 查询数据统计数量
        /// </summary>
        /// <typeparam name="T">过滤器类型</typeparam>
        /// <param name="sql">统计sql语句</param>
        /// <param name="filter">查询过滤器</param>
        /// <returns></returns>
        int GetDataCount<T>(string sql, IQueryFilter filter)
            where T : IQueryFilter, new()
        {
            MySqlParameter[] paras;
            sql += (filter ?? new T()).ToSqlWhere(out paras);

            var result = MySqlHelper.ExecuteScalar(sql, paras);
            return result == DBNull.Value ? 0 : Convert.ToInt32(result);
        }

        #endregion

        #region 非分页查询模板

        /// <summary>
        /// 查询非分页数据列表
        /// </summary>
        /// <typeparam name="TResult">数据类型</typeparam>
        /// <typeparam name="TFilter">过滤器类型</typeparam>
        /// <param name="sql">查询Sql语句</param>
        /// <param name="filter">查询过滤器</param>
        /// <returns></returns>
        public List<TResult> GetData<TResult, TFilter>(string sql, IQueryFilter filter)
            where TResult : class,new()
            where TFilter : IQueryFilter, new()
        {
            List<TResult> list = new List<TResult>();

            MySqlParameter[] paras;
            sql += (filter ?? new TFilter()).ToSqlWhere(out paras);
            list = MySqlHelper.ExecuteReader<TResult>(sql, paras);

            return list;
        }

        #endregion

        #endregion

        public List<Accounts> GetAccounts(AccountFilter filter)
        {
            string sql = "select a.*,u.UserName as 'CreaterName' from t_account a join t_user u on a.CreaterId=u.Id";
            return GetData<Accounts, AccountFilter>(sql, filter);
        }

        public List<Bookkeeping> GetBookkeeping(int pageIndex, int pageSize, BookkeepingFilter filter)
        {
            string countSql = "SELECT count(b.Id) from t_bookkeeping b JOIN t_user u ON b.CreaterId = u.Id";

            string querySql = "SELECT b.Id, Time, dvt. VALUE AS 'VoucherType', VoucherNum, Abstract, Amount, Direction, Balance, u.UserName AS 'CreaterName', b.CreateTime, AbandonReason,b.Status FROM t_bookkeeping b JOIN t_user u ON b.CreaterId = u.Id JOIN t_dictionary dvt ON b.VoucherType = dvt.Id";

            return GetPagedData1<Bookkeeping, BookkeepingFilter>(pageIndex, pageSize, countSql, querySql, filter);
        }

        public int GetBookkeepingCount(BookkeepingFilter filter)
        {
            string countSql = "SELECT count(b.Id) FROM t_bookkeeping b JOIN t_user u ON b.CreaterId = u.Id JOIN t_dictionary dvt ON b.VoucherType = dvt.Id";
            return GetDataCount<BookkeepingFilter>(countSql, filter);
        }


        public List<Department> GetDepartment(DepartmentFilter filter)
        {
            string sql = "select d.Id,d.Name,d.IsEnabled ,d.`Desc`,d.CreateTime,u.userName as CreaterName from t_department d JOIN t_user u ON d.CreaterId = u.Id";
            return GetData<Department, DepartmentFilter>(sql, filter);
        }

        public List<Duty> GetDuty(DutyFilter filter)
        {
            string sql = "SELECT d.id, d. NAME, d.`Desc`, dp.`Name` AS DepartmentName, d.IsEnabled, u.UserName AS CreaterName, d.CreateTime FROM t_duty d JOIN t_department dp ON d.DepartmentId = dp.Id JOIN t_user u ON d.CreaterId = u.Id";
            return GetData<Duty, DutyFilter>(sql, filter);
        }

        public List<User> GetUser(int pageIndex, int pageSize, UserFilter filter)
        {
            string countSql = "SELECT  count(u.id) FROM T_user u LEFT JOIN T_user ur ON u.CreaterId = ur.id";

            string querySql = "SELECT u.*, ur.UserName AS CreaterName FROM T_user u LEFT JOIN T_user ur ON u.CreaterId = ur.id";

            return GetPagedData1<User, UserFilter>(pageIndex, pageSize, countSql, querySql, filter);
        }

        public int GetUserCount(UserFilter filter)
        {
            string countSql = "SELECT  count(u.id) FROM T_user u LEFT JOIN T_user ur ON u.CreaterId = ur.id";
            return GetDataCount<UserFilter>(countSql, filter);
        }
    }
}
