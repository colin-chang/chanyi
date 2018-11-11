using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Multiplying
{
    /// <summary>
    /// 断奶
    /// </summary>
    public class Ablactation : BaseModelWithPrincipal
    {
        public string SheepId { get; set; }
        public string SerialNumber { get; set; }

        public GenderEnum Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public float? BirthWeight { get; set; }

        public DateTime AblactationDate { get; set; }

        public float AblactationWeight { get; set; }
    }
}
