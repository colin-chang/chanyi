using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Input
{
    public class OtherInOutFilter : BaseModelWithPrincipalFilter
    {
        public float? MaxAmount { get; set; }
        public float? MinAmount { get; set; }

        //public string Name { get; set; }

        public InOutWarehouseDirectionEnum? Direction { get; set; }
        /// <summary>
        /// 出库去向
        /// </summary>
        public OutWarehouseDispositonEnum? Dispositon { get; set; }
        public DateTime? StartOperationDate { get; set; }
        public DateTime? EndOperationDate { get; set; }
        
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

                if (Direction != null)
                {
                    list.Add("\"Direction\"=@Direction");
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
