using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.HR
{
    /// <summary>
    /// 合作人基类
    /// </summary>
    public class Cooperater : BaseModelWithPrincipal
    {
        public string Name { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactInfo { get; set; }
    }
}
