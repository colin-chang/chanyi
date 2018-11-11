using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Doamin
{
    public class Log
    {
        public string Id { get; private set; }

        public string Operation { get; private set; }

        public string OperatorId { get; private set; }

        public DateTime Time { get; private set; }
    }
}
