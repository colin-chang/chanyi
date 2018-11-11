using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Multiplying
{
    public class MatingCount
    {
        public string SerialNumber { get; set; }
        public int Count { get; set; }
        public string BreedName { get; set; }
        public SheepStatusEnum Status { get; set; }
        public string SheepfoldName { get; set; }
    }
}
