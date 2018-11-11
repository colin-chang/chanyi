using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.System
{
    public class SheepRemaindful
    {
        public string SerialNumber { get; set; }
        public string Breed { get; set; }
        public OriginEnum Origin { get; set; }
        public string Sheepfold { get; set; }
    }
}
