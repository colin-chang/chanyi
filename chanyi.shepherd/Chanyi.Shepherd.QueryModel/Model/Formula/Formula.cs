using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Formula
{
    /// <summary>
    /// 配方
    /// </summary>
    public class Formula : BaseModelWithPrincipal
    {
        public string Name { get; set; }

        /// <summary>
        /// 适用于
        /// </summary>
        public string ApplyTo { get; set; }

        /// <summary>
        /// 不良反应
        /// </summary>
        public string SideEffect { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
