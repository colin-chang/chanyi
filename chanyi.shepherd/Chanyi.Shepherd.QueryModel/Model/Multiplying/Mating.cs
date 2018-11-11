using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Multiplying
{
    /// <summary>
    /// 配种
    /// </summary>
    public class Mating : BaseModelWithPrincipal
    {
        /// <summary>
        /// 母羊Id
        /// </summary>
        public string FemaleId { get; set; }

        /// <summary>
        /// 母羊编号
        /// </summary>
        public string FemaleSerialNumber { get; set; }

        /// <summary>
        /// 公羊Id
        /// </summary>
        public string MaleId { get; set; }

        /// <summary>
        /// 公羊编号
        /// </summary>
        public string MaleSerialNumber { get; set; }

        /// <summary>
        /// 配种日期
        /// </summary>
        public DateTime MatingDate { get; set; }

        /// <summary>
        /// 是否进行预产期提醒
        /// </summary>
        public bool IsRemindful { get; set; }

        /// <summary>
        /// 母羊品种编号
        /// </summary>
        public string FemaleBreed { get; set; }

        /// <summary>
        /// 公羊品种编号
        /// </summary>
        public string MaleBreed { get; set; }

        /// <summary>
        /// 预产期（expected date of childbirth）
        /// </summary>
        public DateTime EDC { get; set; }

        public bool IsDel { get; set; }
    }
}
