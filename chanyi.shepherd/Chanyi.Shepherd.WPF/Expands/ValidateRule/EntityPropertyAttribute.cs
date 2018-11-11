using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.Expands.ValidateRule
{
    /// <summary>
    /// 搜索绑定字段标记(仅用于展示展示搜索条件，不能Clear)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EntityPropertyAttribute:Attribute
    {
    }
}
