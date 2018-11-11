using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Assist
{
    /// <summary>
    /// 系谱
    /// </summary>
    public class FamilyTree
    {
        public string Id { get; set; }
        public string SerialNumber { get; set; }
        public GenderEnum Gender { get; set; }
        public string FatherId { get; set; }
        public string MotherId { get; set; }

        public SheepStatusEnum Status { get; set; }

        /// <summary>
        /// 上几代
        /// </summary>
        public int Generation { get; set; }
    }
}
