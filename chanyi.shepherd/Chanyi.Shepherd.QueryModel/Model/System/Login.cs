using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.System
{
    public class Login
    {
        public int? ErrorTimes { get; set; }

        public DateTime? LastErrorTime { get; set; }

        public bool IsEnabled { get; set; }
    }
}
