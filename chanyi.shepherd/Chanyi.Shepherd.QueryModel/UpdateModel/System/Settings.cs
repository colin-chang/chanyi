using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.UpdateModel.System
{
    public abstract class Settings
    {
        public string Id { get; set; }
        public string OperatorId { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 是否提醒
        /// </summary>
        public bool IsRemaindful { get; set; }

        public string Value { get; set; }

        public SettingsEnum Type { get; set; }
    }
}
