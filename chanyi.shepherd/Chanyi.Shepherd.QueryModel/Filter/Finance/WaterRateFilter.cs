using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public class WaterRateFilter : ExpenditureFilter
    {
        public float? MaxAmount { get; set; }
        public float? MinAmount { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();

                list.AddRange(base.GetBaseSqlWhere(out pms));

                if (MaxAmount != null)
                {
                    list.Add("\"Amount\"<=@MaxAmount");
                    pms.AddWithValue("MaxAmount", MaxAmount);
                }
                if (MinAmount != null)
                {
                    list.Add("\"Amount\">=@MinAmount");
                    pms.AddWithValue("MinAmount", MinAmount);
                }
                return list;
            };
        }
    }
}
