using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using Spring.Data.Common;

using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public class BuyFeedFilter : BuyInputsFilter
    {
        public string AreaId { get; set; }
        public string TypeId { get; set; }
        /// <summary>
        /// T_InventoryDict的Category
        /// </summary>
        public string Category { get { return ConfigurationManager.AppSettings["feedTypeCategory"]; } }
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();

                list.AddRange(base.GetBaseSqlWhere(out pms));

                if (!string.IsNullOrWhiteSpace(NameId))
                {
                    list.Add("f.\"NameId\"=@NameId");
                    pms.AddWithValue("NameId", NameId);
                }

                if (!string.IsNullOrWhiteSpace(AreaId))
                {
                    list.Add("f.\"AreaId\"=@AreaId");
                    pms.AddWithValue("AreaId", AreaId);
                }

                if (!string.IsNullOrWhiteSpace(TypeId))
                {
                    list.Add("f.\"TypeId\"=@TypeId");
                    pms.AddWithValue("TypeId", TypeId);
                }


                if (!string.IsNullOrWhiteSpace(Category))
                {
                    list.Add("d.\"Category\"=@Category");
                    pms.AddWithValue("Category", Category);
                }
                return list;
            };
        }
    }
}
