using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
using System.Configuration;
namespace Chanyi.Shepherd.QueryModel.Filter.Input
{
    public class MedicineInOutFilter : InOutFilter
    {
        public string ManufacturerId { get; set; }
        public string TypeId { get; set; }
        public string Category { get { return ConfigurationManager.AppSettings["medicineTypeCategory"]; } }
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
             {
                 List<string> list = new List<string>();
                 pms = Template.CreateDbParameters();

                 #region 基础
                 if (!string.IsNullOrWhiteSpace(Id))
                 {
                     list.Add("w.\"Id\"=@Id");
                     pms.AddWithValue("Id", Id);
                 }
                 if (!string.IsNullOrWhiteSpace(OperatorId))
                 {
                     list.Add("w.\"OperatorId\"=@OperatorId");
                     pms.AddWithValue("OperatorId", OperatorId);
                 }
                 if (StartCreateTime != null)
                 {
                     list.Add("w.\"CreateTime\">=@StartCreateTime");
                     pms.AddWithValue("StartCreateTime", StartCreateTime);
                 }
                 if (EndCreateTime != null)
                 {
                     list.Add("w.\"CreateTime\"<=@EndCreateTime");
                     pms.AddWithValue("EndCreateTime", EndCreateTime);
                 }
                 if (!string.IsNullOrWhiteSpace(Remark))
                 {
                     list.Add("w.\"Remark\" like @Remark");
                     pms.AddWithValue("Remark", Remark.Wrap("%"));
                 }

                 if (!string.IsNullOrWhiteSpace(PrincipalId))
                 {
                     list.Add("w.\"PrincipalId\" = @PrincipalId");
                     pms.AddWithValue("PrincipalId", PrincipalId);
                 }
                 #endregion


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

                 if (Direction != null)
                 {
                     list.Add("w.\"Direction\"=@Direction");
                     pms.AddWithValue("Direction", (int)Direction);
                 }

                 if (StartOperationDate != null)
                 {
                     list.Add("w.\"OperationDate\">=@StartOperationDate");
                     pms.AddWithValue("StartOperationDate", StartOperationDate);
                 }
                 if (EndOperationDate != null)
                 {
                     list.Add("w.\"OperationDate\"<=@EndOperationDate");
                     pms.AddWithValue("EndOperationDate", EndOperationDate);
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
