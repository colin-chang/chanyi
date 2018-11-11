using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Input
{
    public class FeedInOutFilter : InOutFilter
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
                    list.Add("\"Amount\"<=@MaxAmount");
                    pms.AddWithValue("MaxAmount", MaxAmount);
                }
                if (MinAmount != null)
                {
                    list.Add("\"Amount\">=@MinAmount");
                    pms.AddWithValue("MinAmount", MinAmount);
                }

                if (!string.IsNullOrWhiteSpace(NameId))
                {
                    list.Add("\"NameId\"=@NameId");
                    pms.AddWithValue("NameId", NameId);
                }

                if (Direction != null)
                {
                    list.Add("\"Direction\"=@Direction");
                    pms.AddWithValue("Direction", (int)Direction);
                }

                if (!string.IsNullOrWhiteSpace(AreaId))
                {
                    list.Add("\"AreaId\"=@AreaId");
                    pms.AddWithValue("AreaId", AreaId);
                }

                if (!string.IsNullOrWhiteSpace(TypeId))
                {
                    list.Add("\"TypeId\"=@TypeId");
                    pms.AddWithValue("TypeId", TypeId);
                }

                if (StartOperationDate != null)
                {
                    list.Add("\"OperationDate\">=@StartOperationDate");
                    pms.AddWithValue("StartOperationDate", StartOperationDate);
                }
                if (EndOperationDate != null)
                {
                    list.Add("\"OperationDate\"<=@EndOperationDate");
                    pms.AddWithValue("EndOperationDate", EndOperationDate);
                }
                if (!string.IsNullOrWhiteSpace(Category))
                {
                    list.Add("d.\"Category\"=@Category");
                    pms.AddWithValue("Category", Category);
                }


                if (Dispositon != null)
                {
                    list.Add("\"Dispositon\"=@Dispositon");
                    pms.AddWithValue("Dispositon", (int)Dispositon);
                }
                return list;
            };
        }
    }
}
