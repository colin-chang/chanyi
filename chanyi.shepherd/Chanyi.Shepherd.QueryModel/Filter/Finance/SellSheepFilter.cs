using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public class SellSheepFilter : BaseModelWithPrincipalFilter
    {
        public string SheepId { get; set; }

        public GrowthStageEnum? GrowthStage { get; set; }

        public GenderEnum? Gender { get; set; }

        public string BreedId { get; set; }

        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// 购买人ID
        /// </summary>
        public string PurchaserId { get; set; }

        public DateTime? StartOperationDate { get; set; }
        public DateTime? EndOperationDate { get; set; }

        /// <summary>
        /// 批次编号
        /// </summary>
        public string BatchId { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
             {
                 List<string> list = new List<string>();
                 pms = Template.CreateDbParameters();

                 #region 基本的
                 if (!string.IsNullOrWhiteSpace(Id))
                 {
                     list.Add("s.\"Id\"=@Id");
                     pms.AddWithValue("Id", Id);
                 }
                 if (!string.IsNullOrWhiteSpace(OperatorId))
                 {
                     list.Add("s.\"OperatorId\"=@OperatorId");
                     pms.AddWithValue("OperatorId", OperatorId);
                 }
                 if (StartCreateTime != null)
                 {
                     list.Add("s.\"CreateTime\">=@StartCreateTime");
                     pms.AddWithValue("StartCreateTime", StartCreateTime);
                 }
                 if (EndCreateTime != null)
                 {
                     list.Add("s.\"CreateTime\"<=@EndCreateTime");
                     pms.AddWithValue("EndCreateTime", EndCreateTime);
                 }
                 if (!string.IsNullOrWhiteSpace(Remark))
                 {
                     list.Add("s.\"Remark\" like @Remark");
                     pms.AddWithValue("Remark", Remark.Wrap("%"));
                 }

                 if (!string.IsNullOrWhiteSpace(PrincipalId))
                 {
                     list.Add("s.\"PrincipalId\" = @PrincipalId");
                     pms.AddWithValue("PrincipalId", PrincipalId);
                 }
                 #endregion


                 if (MaxPrice != null)
                 {
                     list.Add("s.\"Price\"<=cast(@MaxPrice as money)");
                     pms.AddWithValue("MaxPrice", MaxPrice);
                 }
                 if (MinPrice != null)
                 {
                     list.Add("s.\"Price\">=cast(@MinPrice as money)");
                     pms.AddWithValue("MinPrice", MinPrice);
                 }


                 if (!string.IsNullOrWhiteSpace(PurchaserId))
                 {
                     list.Add("s.\"PurchaserId\"=@PurchaserId");
                     pms.AddWithValue("PurchaserId", PurchaserId);
                 }


                 if (StartOperationDate != null)
                 {
                     list.Add("s.\"OperationDate\">=@StartOperationDate");
                     pms.AddWithValue("StartOperationDate", StartOperationDate);
                 }
                 if (EndOperationDate != null)
                 {
                     list.Add("s.\"OperationDate\"<=@EndOperationDate");
                     pms.AddWithValue("EndOperationDate", EndOperationDate);
                 }

                 if (!string.IsNullOrWhiteSpace(SheepId))
                 {
                     list.Add("ss.\"SheepId\"=@SheepId");
                     pms.AddWithValue("SheepId", SheepId);
                 }


                 if (Gender != null)
                 {
                     list.Add("sp.\"Gender\"=@Gender");
                     pms.AddWithValue("Gender", (int)Gender);
                 }


                 if (GrowthStage != null)
                 {
                     list.Add("sp.\"GrowthStage\"=@GrowthStage");
                     pms.AddWithValue("GrowthStage", (int)GrowthStage);
                 }

                 if (!string.IsNullOrWhiteSpace(BreedId))
                 {
                     list.Add("sp.\"BreedId\"=@BreedId");
                     pms.AddWithValue("BreedId", BreedId);
                 }

                 if (!string.IsNullOrWhiteSpace(BatchId))
                 {
                     list.Add("ss.\"SellSheepBatchId\"=@BatchId");
                     pms.AddWithValue("BatchId", BatchId);
                 }


                 return list;
             };
        }
    }
}
