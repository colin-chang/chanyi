using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.AddModel.BaseInfo
{
    public class Sheep
    {
        public string SerialNumber { get; set; }

        public GenderEnum Gender { get; set; }

        public string SheepfoldId { get; set; }

        public float? BirthWeight { get; set; }

        public string Remark { get; set; }
    }
}
