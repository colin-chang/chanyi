using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Input
{
    public class OtherInventoryFilter : BaseFilter
    {
        //public string Name { get; set; }
        public string Id { get; set; }

        public float? MaxAmount { get; set; }
        public float? MinAmount { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("oi.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }

                if (MaxAmount != null)
                {
                    list.Add("oi.\"Amount\"<=@MaxAmount");
                    pms.AddWithValue("MaxAmount", MaxAmount);
                }
                if (MinAmount != null)
                {
                    list.Add("oi.\"Amount\">=@MinAmount");
                    pms.AddWithValue("MinAmount", MinAmount);
                }

                return list;
            };
        }
    }
}
