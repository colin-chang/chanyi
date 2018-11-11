using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.BindingModel
{
    /// <summary>
    /// 圈舍以及圈舍内的羊只数量
    /// </summary>
    public class SheepfoldSheepCountBind
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
