using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.QueryService.Core.Dto
{
    /// <summary>
    /// 现金日记账
    /// </summary>
    [DataContract]
    public class Bookkeeping
    {

        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// 日期时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 凭证种类
        /// </summary>
        [DataMember]
        public string VoucherType { get; set; }

        /// <summary>
        /// 凭证号数
        /// </summary>
        [DataMember]
        public string VoucherNum { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [DataMember]
        public string Abstract { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// 借贷方向
        /// </summary>
        [DataMember]
        public int Direction { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        [DataMember]
        public decimal Balance { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public string CreaterName { get; set; }

        /// <summary>
        /// 记账时间
        /// </summary>
        [DataMember]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 当前记录废弃后指向的新纪录ID
        /// </summary>
        [DataMember]
        public string ReferenceId { get; set; }

        /// <summary>
        /// 废弃原因
        /// </summary>
        [DataMember]
        public string AbandonReason { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }
    }
}
