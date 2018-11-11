using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.Expands.ValidateRule
{
    /// <summary>
    /// 条件校验
    /// </summary>
    public class ConditionalVerifyAttribute:Attribute
    {
        /// <summary>
        /// 校验属性名称
        /// </summary>
        public string VerifiedPropertyName { get; set; }

        /// <summary>
        /// 条件校验属性所属类类型
        /// </summary>
        public Type VerifiedType { get; set; }

        /// <summary>
        /// 校验规则
        /// </summary>
        public Func<bool> VerifyRule { get; set; }
    }
}
