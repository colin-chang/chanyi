using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public class BuySheepFilter : ExpenditureFilter
    {
        public string SheepId { get; set; }

        public string Source { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                list.AddRange(base.GetBaseSqlWhere(out pms));

                if (!string.IsNullOrWhiteSpace(SheepId))
                {
                    list.Add("\"SheepId\"=@SheepId");
                    pms.AddWithValue("SheepId", SheepId);
                }

                if (!string.IsNullOrWhiteSpace(Source))
                {
                    list.Add("\"Source\"=@Source");
                    pms.AddWithValue("Source", Source);
                }

                return list;
            };
        }
    }
}
