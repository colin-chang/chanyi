using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.ReportForms
{
    /// <summary>
    /// 每个月每种饲料的使用量与库存量
    /// </summary>
    public class FeedReport
    {
        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }

        public float Amount { get; set; }

        /// <summary>
        /// 使用量（出库）
        /// </summary>
        public float Used { get; set; }
        
        /// <summary>
        /// 入库量（入库）
        /// </summary>
        public float Storage  { get; set; }

        /// <summary>
        /// 当前月库存量
        /// </summary>
        public float Odd { get; set; }


        public string Name { get; set; }
        public string Type { get; set; }
        public string Area { get; set; }

        public InOutWarehouseDirectionEnum Direction { get; set; }

        private string fullName;

        public string FullName
        {
            get
            {
                return string.Format("{0}-{1}-{2}", this.Name.Trim(), this.Type.Trim(), this.Area.Trim());
            }
            set { fullName = value; }
        }

    }
}
