using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Breeding
{
    public abstract class Assess : BaseModelWithPrincipal
    {
        public string SheepId { get; set; }

        public string SerialNumber { get; set; }

        public float Weight { get; set; }

        /// <summary>
        /// 体型评分
        /// </summary>
        public float HabitusScore { get; set; }

        public DateTime AssessDate { get; set; }

        public DateTime? Birthday { get; set; }

        public GenderEnum Gender { get; set; }

        public string BreedName { get; set; }

        /// <summary>
        /// TODO:等待公式，需要修改sql
        /// </summary>
        public string TotalScore { get; set; }
    }
}
