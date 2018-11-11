using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.BaseInfo
{
    public class Sheep : BaseModelWithPrincipal
    {
        public string SerialNumber { get; set; }

        public GenderEnum Gender { get; set; }

        public GrowthStageEnum GrowthStage { get; set; }

        public string BreedId { get; set; }

        public string SheepfoldId { get; set; }

        public float? BirthWeight { get; set; }

        /// <summary>
        /// 同胎羔羊数
        /// </summary>
        public int CompatriotNumber { get; set; }

        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 断奶重
        /// </summary>

        public float? AblactationWeight { get; set; }

        public DateTime? AblactationDate { get; set; }

        public OriginEnum Origin { get; set; }

        public string FatherId { get; set; }

        public string MotherId { get; set; }

        public SheepStatusEnum Status { get; set; }

        public string MotherSerialNumber { get; set; }

        public string FatherSerialNumber { get; set; }

        public string BreedName { get; set; }

        public string SheepfoldName { get; set; }
    }
}
