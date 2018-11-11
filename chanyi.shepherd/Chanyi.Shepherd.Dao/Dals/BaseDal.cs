using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

using Spring.Data.Generic;
using Spring.Data.Common;

using Chanyi.SecurityUtility;
using Chanyi.Shepherd.Dao.Mappers;
using Chanyi.Shepherd.IDao;
using Chanyi.Shepherd.QueryModel;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal : AdoDaoSupport, IDal
    {
        public void Initialize()
        {
            string sql = "select 'Initialize'";
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql);
        }

        #region 调用方法
        //List<TResult> GetData<TResult, TFilter, TRowMapper>(string sql, IQueryFilter filter)
        //    where TResult : class,new()
        //    where TFilter : IQueryFilter, new()
        //    where TRowMapper : IRowMapper<TResult>, new()
        //{
        //    List<TResult> list = new List<TResult>();

        //    IDbParameters pms;
        //    sql += (filter ?? new TFilter()).ToSqlWhere(out pms);
        //    list = AdoTemplate.QueryWithRowMapper<TResult>(CommandType.Text, sql, new TRowMapper(), pms).ToList();

        //    return list;
        //}

        /// <summary>
        /// 绑定查询
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        List<TResult> GetData<TResult>(string sql, IDbParameters pms)
            where TResult : class, new()
        {
            List<TResult> list = new List<TResult>();

            list = AdoTemplate.QueryWithRowMapper<TResult>(CommandType.Text, sql, new BaseRowMapper<TResult>(), pms).ToList();

            return list;
        }

        /// <summary>
        /// 非分页查询
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TFilter"></typeparam>
        /// <param name="sql"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<TResult> GetData<TResult, TFilter>(string sql, IQueryFilter filter)
            where TResult : class, new()
            where TFilter : IQueryFilter, new()
        {
            List<TResult> list = new List<TResult>();

            IDbParameters pms;
            sql += (filter ?? new TFilter()).ToSqlWhere(out pms);
            list = AdoTemplate.QueryWithRowMapper<TResult>(CommandType.Text, sql, new BaseRowMapper<TResult>(), pms).ToList();

            return list;
        }

        /// <summary>
        /// sql语句中包含聚合语句，where不能放在最后
        /// 仅支持一个参数的情况下
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TFilter"></typeparam>
        /// <param name="sql"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<TResult> GetDataWithFormate<TResult, TFilter>(string sql, IQueryFilter filter)
            where TResult : class, new()
            where TFilter : IQueryFilter, new()
        {
            List<TResult> list = new List<TResult>();

            IDbParameters pms;
            sql = string.Format(sql, (filter ?? new TFilter()).ToSqlWhere(out pms));
            list = AdoTemplate.QueryWithRowMapper<TResult>(CommandType.Text, sql, new BaseRowMapper<TResult>(), pms).ToList();

            return list;
        }

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
        List<TResult> GetPagedData<TResult, TFilter>(int pageIndex, int pageSize, out int totalCount, string countSql, string querySql, IQueryFilter filter)
            where TResult : class, new()
            where TFilter : IQueryFilter, new()
        {
            List<TResult> list = new List<TResult>();

            IDbParameters pms;
            string where = (filter ?? new TFilter()).ToSqlWhere(out pms);
            querySql += where;
            countSql += where;

            pageSize = pageSize < 1 ? 20 : pageSize;

            totalCount = GetDataCount(countSql, pms);

            int totalPages = (int)Math.Ceiling(totalCount * 1.0 / pageSize);
            totalPages = totalPages < 1 ? 1 : totalPages;
            pageIndex = pageIndex < 1 ? 1 : (pageIndex > totalPages ? totalPages : pageIndex);

            string sql = string.Format("select * from ({0}) as sr where sr.\"rownum\" BETWEEN {1} and {2}", querySql, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);

            list = AdoTemplate.QueryWithRowMapper<TResult>(CommandType.Text, sql, new BaseRowMapper<TResult>(), pms).ToList();

            return list;
        }

        /// <summary>
        /// 查询数据统计数量
        /// </summary>
        /// <typeparam name="TFilter">过滤器类型</typeparam>
        /// <param name="sql">统计sql语句</param>
        /// <param name="filter">查询过滤器</param>
        /// <returns></returns>
        int GetDataCount(string sql, IDbParameters pms)
        {
            var result = AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
            return result == DBNull.Value ? 0 : Convert.ToInt32(result);
        }

        /// <summary>
        /// 获取规定行数的数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TFilter"></typeparam>
        /// <param name="rowsCount">要查询出的行数</param>
        /// <param name="querySql"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<TResult> GetRuledRowsData<TResult, TFilter>(int rowsCount, string querySql, IQueryFilter filter)
            where TResult : class, new()
            where TFilter : IQueryFilter, new()
        {
            List<TResult> list = new List<TResult>();

            IDbParameters pms;
            querySql += (filter ?? new TFilter()).ToSqlWhere(out pms);

            string sql = string.Format("select * from ({0}) as sr where sr.\"rownum\"<={1}", querySql, rowsCount);

            list = AdoTemplate.QueryWithRowMapper<TResult>(CommandType.Text, sql, new BaseRowMapper<TResult>(), pms).ToList();

            return list;
        }

        /// <summary>
        /// sql语句中包含聚合语句，where不能放在最后
        /// 仅支持一个参数的情况下
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TFilter"></typeparam>
        /// <param name="rowsCount"></param>
        /// <param name="querySql"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<TResult> GetRuledRowsDataWithFormate<TResult, TFilter>(int rowsCount, string querySql, IQueryFilter filter)
            where TResult : class, new()
            where TFilter : IQueryFilter, new()
        {
            List<TResult> list = new List<TResult>();

            IDbParameters pms;
            querySql = string.Format(querySql, (filter ?? new TFilter()).ToSqlWhere(out pms));

            string sql = string.Format("select * from ({0}) as sr where sr.\"rownum\"<={1}", querySql, rowsCount);

            list = AdoTemplate.QueryWithRowMapper<TResult>(CommandType.Text, sql, new BaseRowMapper<TResult>(), pms).ToList();

            return list;
        }

        #endregion

        #region 辅助方法

        public string GetPwd(string password)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ConfigurationManager.AppSettings.Get("pwdsaltprefix"));
            sb.Append(password);
            sb.Append(ConfigurationManager.AppSettings.Get("pwdsaltsuffix"));
            return SecurityHelper.GetSHA256FromString(sb.ToString());
        }

        #endregion

        string feedNameCategory = ConfigurationManager.AppSettings["feedNameCategory"];
        string medicineNameCategory = ConfigurationManager.AppSettings["medicineNameCategory"];
        string feedTypeCategory = ConfigurationManager.AppSettings["feedTypeCategory"];
        string medicineTypeCategory = ConfigurationManager.AppSettings["medicineTypeCategory"];
    }
}
