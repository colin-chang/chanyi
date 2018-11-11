using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.System
{
    /// <summary>
    /// 参数配置修改日志
    /// </summary>
    public class SettingsLog : BaseModel
    {
        public string SettingsId { get; set; }

        public string Category { get; set; }

        public string NewValue { get; set; }
    }
}
