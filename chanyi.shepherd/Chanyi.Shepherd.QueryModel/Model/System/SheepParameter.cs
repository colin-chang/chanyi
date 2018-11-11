using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.System
{
    /// <summary>
    /// 养只生理参数
    /// </summary>
    public class SheepParameter : Settings
    {
        /// <summary>
        /// 提醒日期浮动范围
        /// </summary>
        public int Range { get; set; }
    }
}
