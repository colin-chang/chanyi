using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Chanyi.ERP.QueryService.Core
{
    /// <summary>
    /// 查询过滤条件
    /// </summary>
    public interface IQueryFilter
    {
        /// <summary>
        /// 将当期对象格式化为Sql过滤的Where条件语句
        /// </summary>
        /// <returns></returns>
        string ToSqlWhere(out MySqlParameter[] paras);
    }
}
