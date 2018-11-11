using Spring.Context;
using Spring.Context.Support;
using Spring.Data.Common;
using Spring.Data.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chanyi.Shepherd.QueryModel.Filter;

namespace Chanyi.Shepherd.QueryModel
{
    public abstract class BaseFilter : IQueryFilter
    {
        private GeneratorDelegate generator;

        protected AdoTemplate Template { get { return CallBuffer.AdoTemplate; } }

        public BaseFilter()
        {
            generator = CreateGenerator();
        }

        protected abstract GeneratorDelegate CreateGenerator();

        public virtual string ToSqlWhere(out IDbParameters pms)
        {
            if (FilterHelper.IsObjectEmpty(this) || generator == null)
            {
                pms = Template.CreateDbParameters();
                return string.Empty;
            }

            List<string> listWhere = new List<string>();
            listWhere.AddRange(generator(out pms));

            return listWhere.Count > 0 ? " where " + string.Join(" and ", listWhere) : string.Empty;
        }
    }
}
