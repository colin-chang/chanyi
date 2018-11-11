using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Group
{
    public class DeathManage : BaseModelWithPrincipal
    {
        public string SheepId { get; set; }

        public string SerialNumber { get; set; }

        public GenderEnum Gender { get; set; }

        public string BreedName { get; set; }

        public GrowthStageEnum GrowthStage { get; set; }

        //public string SheepfoldName { get; set; }

        public string Reason { get; set; }

        public DeathDisposeEnum Dispose { get; set; }

        public DateTime DeathDate { get; set; }

        public bool IsDel { get; set; }
    }
}
