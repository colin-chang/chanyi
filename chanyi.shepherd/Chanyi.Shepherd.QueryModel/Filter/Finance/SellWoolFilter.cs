using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public class SellWoolFilter : SellFilter
    {
        public float? MaxAmount { get; set; }
        public float? MinAmount { get; set; }
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                list.AddRange(base.GetSellSqlWhere(out pms));


                if (MaxAmount != null)
                {
                    list.Add("w.\"Amount\"<=@MaxAmount");
                    pms.AddWithValue("MaxAmount", MaxAmount);
                }
                if (MinAmount != null)
                {
                    list.Add("w.\"Amount\">=@MinAmount");
                    pms.AddWithValue("MinAmount", MinAmount);
                }

                return list;
            };
        }
    }
}
