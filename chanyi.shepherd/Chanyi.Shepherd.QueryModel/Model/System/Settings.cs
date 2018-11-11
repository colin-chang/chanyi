using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.System
{
    public abstract class Settings : BaseModel
    {
        /// <summary>
        /// 是否提醒
        /// </summary>
        public bool IsRemaindful { get; set; }

        public string Value { get; set; }

        public SettingsEnum Type { get; set; }
    }
}
