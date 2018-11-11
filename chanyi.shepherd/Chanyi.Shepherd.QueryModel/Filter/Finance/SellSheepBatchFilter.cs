using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public class SellSheepBatchFilter : SellFilter
    {
        /// <summary>
        /// 批次编号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 出售数量
        /// </summary>
        public int? MaxSellCount { get; set; }
        public int? MinSellCount { get; set; }

        public float? MaxTotalWeight { get; set; }
        public float? MinTotalWeight { get; set; }


        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
             {
                 List<string> list = new List<string>();
                 list.AddRange(base.GetSellSqlWhere(out pms));

                 if (!string.IsNullOrWhiteSpace(SerialNumber))
                 {
                     list.Add("ssb.\"SerialNumber\" like @SerialNumber");
                     pms.AddWithValue("SerialNumber", SerialNumber.Wrap("%"));
                 }

                 if (MaxSellCount != null)
                 {
                     list.Add("ssb.\"SellCount\"<=@MaxSellCount");
                     pms.AddWithValue("MaxSellCount", MaxSellCount);
                 }
                 if (MinSellCount != null)
                 {
                     list.Add("ssb.\"SellCount\">=@MinSellCount");
                     pms.AddWithValue("MinSellCount", MinSellCount);
                 }

                 if (MaxTotalWeight != null)
                 {
                     list.Add("ssb.\"TotalWeight\"<=@MaxTotalWeight");
                     pms.AddWithValue("MaxTotalWeight", MaxTotalWeight);
                 }
                 if (MinTotalWeight != null)
                 {
                     list.Add("ssb.\"TotalWeight\">=@MinTotalWeight");
                     pms.AddWithValue("MinTotalWeight", MinTotalWeight);
                 }

                 return list;
             };
        }
    }
}
