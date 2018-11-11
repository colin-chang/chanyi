using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Assist
{
    /// <summary>
    /// 辅助配种
    /// </summary>
    public class AssistMating
    {
        public string Id { get; set; }
        public string SerialNumber { get; set; }
        public string Sheepfold { get; set; }
        public string Breed { get; set; }
        public OriginEnum Origin { get; set; }
        public GenderEnum Gender { get; set; }
    }
}
