using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.BindingModel
{
    public class SheepBind : BaseBindModel
    {
        public string SerialNumber { get; set; }

        public string BreedId { get; set; }

        public GenderEnum Gender { get; set; }

        public GrowthStageEnum GrowthStage { get; set; }

        public SheepStatusEnum Status { get; set; }

        public string SheepfoldId { get; set; }

        public string SheepfoldName { get; set; }
    }
}
