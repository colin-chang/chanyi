using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Finance
{
    /// <summary>
    /// 出售羊只批次
    /// </summary>
    public class SellSheepBatch : Sell
    {
        /// <summary>
        /// 批次编号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 出售数量
        /// </summary>
        public int SellCount { get; set; }

        public float TotalWeight { get; set; }

    }
}
