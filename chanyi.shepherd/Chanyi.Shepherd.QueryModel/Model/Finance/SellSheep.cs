using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Finance
{
    public class SellSheep : BaseModelWithPrincipal
    {
        public string SheepId { get; set; }

        public string SerialNumber { get; set; }

        //public string Sheepfold { get; set; }

        public string Breed { get; set; }

        public GrowthStageEnum GrowthStage { get; set; }

        public GenderEnum Gender { get; set; }

        public float? Weight { get; set; }

        public decimal Price { get; set; }

        /// <summary>
        /// 购买人ID
        /// </summary>
        public string PurchaserId { get; set; }

        public string Purchaser { get; set; }

        public string Department { get; set; }

        public DateTime OperationDate { get; set; }

        /// <summary>
        /// 批次编号
        /// </summary>
        public string BatchSerialNumber { get; set; }
    }
}
