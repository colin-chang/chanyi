using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.Filter.Multiplying
{
    /// <summary>
    /// 配种次数统计
    /// </summary>
    public class MatingCountFilter : BaseFilter
    {
        public string SheepId { get; set; }
        public DateTime? StartMatingTime { get; set; }
        public DateTime? EndMatingTime { get; set; }
        public int? Count { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();


                //if (!string.IsNullOrWhiteSpace(SheepId))
                //{
                //    list.Add("\"SheepId\"=@SheepId");
                //    pms.AddWithValue("SheepId", SheepId);
                //}


                //if (StartMatingTime != null)
                //{
                //    list.Add("\"MatingTime\">=@StartMatingTime");
                //    pms.AddWithValue("StartMatingTime", StartMatingTime);
                //}
                //if (EndMatingTime != null)
                //{
                //    list.Add("\"MatingTime\"<=@EndMatingTime");
                //    pms.AddWithValue("EndMatingTime", EndMatingTime);
                //}

                //if (Count != null)
                //{
                //    list.Add("\"Count\"=@Count");
                //    pms.AddWithValue("Count", Count);
                //}


                return list;
            };
        }
    }
}
