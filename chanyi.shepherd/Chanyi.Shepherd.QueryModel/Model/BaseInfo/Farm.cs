using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.BaseInfo
{
    /// <summary>
    /// 羊场信息
    /// </summary>
    public class Farm
    {
        public string Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        /// <summary>
        /// 羊编号生成编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        public string BusinessScope { get; set; }

        /// <summary>
        /// 羊场资质
        /// </summary>
        public string Qualifications { get; set; }

        public string Remark { get; set; }
    }
}
