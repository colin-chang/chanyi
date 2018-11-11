using Chanyi.ERP.QueryService.Core.Helper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.QueryService.Core.Filter
{
    [KnownType("GetSubClass")]
    [DataContract]
    public abstract class BaseFilter : IQueryFilter
    {
        static Type[] GetSubClass()
        {
            return Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => typeof(BaseFilter).IsAssignableFrom(t)).ToArray();
        }


        protected abstract GeneratorDelegate CreateGenerator();

        public string ToSqlWhere(out MySqlParameter[] pms)
        {
            GeneratorDelegate generator = CreateGenerator();
            if (FilterHelper.IsObjectEmpty(this) || generator == null)
            {
                pms = null;
                return string.Empty;
            }

            List<MySqlParameter> ps;
            List<string> listWhere = generator(out ps);
            pms = ps.ToArray();

            return " where " + string.Join(" and ", listWhere);
        }
    }

    public delegate List<string> GeneratorDelegate(out List<MySqlParameter> pms);
}
