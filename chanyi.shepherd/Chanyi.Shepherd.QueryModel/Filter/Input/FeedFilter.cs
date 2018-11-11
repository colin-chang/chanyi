using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
using System.Configuration;
namespace Chanyi.Shepherd.QueryModel.Filter.Input
{
    public class FeedFilter : InputFilter
    {
        public string TypeId { get; set; }

        public string AreaId { get; set; }
        /// <summary>
        /// T_InventoryDict的Category
        /// </summary>
        public string Category { get { return ConfigurationManager.AppSettings["feedTypeCategory"]; } }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("fd.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }

                if (!string.IsNullOrWhiteSpace(NameId))
                {
                    list.Add("f.\"NameId\"=@NameId");
                    pms.AddWithValue("NameId", NameId);
                }

                if (!string.IsNullOrWhiteSpace(TypeId))
                {
                    list.Add("f.\"TypeId\"=@TypeId");
                    pms.AddWithValue("TypeId", TypeId);
                }

                if (!string.IsNullOrWhiteSpace(AreaId))
                {
                    list.Add("f.\"AreaId\"=@AreaId");
                    pms.AddWithValue("AreaId", AreaId);
                }

                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
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
