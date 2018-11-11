using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
using System.Configuration;
namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public class BuyMedicineFilter : BuyInputsFilter
    {
        public string ManufacturerId { get; set; }
        public string TypeId { get; set; }
        public string Category { get { return ConfigurationManager.AppSettings["medicineTypeCategory"]; } }
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();

                list.AddRange(base.GetBaseSqlWhere(out pms));


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
