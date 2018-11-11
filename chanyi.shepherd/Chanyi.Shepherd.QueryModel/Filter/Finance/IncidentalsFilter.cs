using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    /// <summary>
    /// 杂费
    /// </summary>
    public class IncidentalsFilter : ExpenditureFilter
    {
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                list.AddRange(base.GetBaseSqlWhere(out pms));

                return list;
            };
        }
    }
}
