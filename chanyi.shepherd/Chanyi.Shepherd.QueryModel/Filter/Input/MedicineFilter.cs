using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Input
{
    public class MedicineFilter : InputFilter
    {
        public string ManufacturerId { get; set; }
        public string TypeId { get; set; }
        /// <summary>
        /// T_InventoryDict的Category
        /// </summary>
        public string Category { get { return ConfigurationManager.AppSettings["medicineTypeCategory"]; } }

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
                    list.Add("m.\"NameId\"=@NameId");
                    pms.AddWithValue("NameId", NameId);
                }

                if (!string.IsNullOrWhiteSpace(ManufacturerId))
                {
                    list.Add("m.\"ManufacturerId\"=@ManufacturerId");
                    pms.AddWithValue("ManufacturerId", ManufacturerId);
                }


                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("md.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(TypeId))
                {
                    list.Add("\"TypeId\"=@TypeId");
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
