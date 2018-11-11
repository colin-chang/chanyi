using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.BindingModel
{
    public class DiseaseBind:SicknessBind
    {
        /// <summary>
        /// 与搜索症状集合的相关度
        /// 与数据库的count映射无法使用int类型
        /// </summary>
        public long? Relevancy { get; set; }
    }
}
