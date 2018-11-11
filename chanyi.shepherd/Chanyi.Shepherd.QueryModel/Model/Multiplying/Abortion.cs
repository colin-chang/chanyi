using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Multiplying
{
    /// <summary>
    /// 流产
    /// </summary>
    public class Abortion : BaseModelWithPrincipal
    {
        public string SheepId { get; set; }

        public string FemaleNumber { get; set; }
        public string MaleNumber { get; set; }

        public string Reason { get; set; }

        public string Dispose { get; set; }

        public DateTime AbortionDate { get; set; }

        public DateTime MatingDate { get; set; }

        public bool IsDel { get; set; }
    }
}
