using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public class SellOtherFilter:SellFilter
    {
        public string Name { get; set; }
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();

                list.AddRange(base.GetSellSqlWhere(out pms));

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    list.Add("o.\"Name\" like @Name");
                    pms.AddWithValue("Name", Name.Wrap("%"));
                }

                return list;
            };
        }
    }
}
