using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.ReportForms
{
    /// <summary>
    /// 羊只繁殖报表
    /// </summary>
    public class MultiplyReport
    {
        public string Month { get; set; }
        /// <summary>
        /// 流产母羊数
        /// </summary>
        public int Abortion { get; set; }
        /// <summary>
        /// 产羔母羊数
        /// </summary>
        public int Delivery { get; set; }
        
        /// <summary>
        /// 产羔总数
        /// </summary>
        public int TotalCount { get; set; }
        public int LiveMaleCount { get; set; }
        public int LiveFemaleCount { get; set; }
        /// <summary>
        /// 顺产数
        /// </summary>
        public int NormalWayCount { get; set; }


        /// <summary>
        /// 分娩率
        /// </summary>
        public string DeliveryRate { get; set; }
        /// <summary>
        /// 流产率
        /// </summary>
        public string AborationRate { get; set; }
        /// <summary>
        /// 顺产率
        /// </summary>
        public string SelfDeliveryRate { get; set; }
        /// <summary>
        /// 产活率
        /// </summary>
        public string LiveRate { get; set; }
        /// <summary>
        /// 产公羔率
        /// </summary>
        public string MaleRate { get; set; }
        /// <summary>
        /// 产母羔率
        /// </summary>
        public string FemaleRate { get; set; }
    }
}
