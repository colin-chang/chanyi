using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.BaseInfo
{
    public class SheepfoldFilter : BaseModelFilter
    {
        //public string Name { get; set; }

        /// <summary>
        /// 羊圈管理员
        /// </summary>
        public string Administrator { get; set; }

        public string SearchSheepfoldName { get; set; }
        public bool SysFlag { get { return false; } }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                #region 基本数据
                //if (!string.IsNullOrWhiteSpace(Id))
                //{
                //    list.Add("s.\"Id\"=@Id");
                //    pms.AddWithValue("Id", Id);
                //}
                //if (!string.IsNullOrWhiteSpace(OperatorId))
                //{
                //    list.Add("s.\"OperatorId\"=@OperatorId");
                //    pms.AddWithValue("OperatorId", OperatorId);
                //}
                //if (StartCreateTime != null)
                //{
                //    list.Add("s.\"CreateTime\">=@StartCreateTime");
                //    pms.AddWithValue("StartCreateTime", StartCreateTime);
                //}
                //if (EndCreateTime != null)
                //{
                //    list.Add("s.\"CreateTime\"<=@EndCreateTime");
                //    pms.AddWithValue("EndCreateTime", EndCreateTime);
                //}
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("s.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                #endregion


                if (!string.IsNullOrWhiteSpace(SearchSheepfoldName))
                {
                    list.Add("s.\"Name\" like @Name");
                    pms.AddWithValue("Name", SearchSheepfoldName.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(Administrator))
                {
                    list.Add("s.\"Administrator\"=@Administrator");
                    pms.AddWithValue("Administrator", Administrator);
                }
                list.Add("s.\"SysFlag\"=@SysFlag");
                pms.AddWithValue("SysFlag", SysFlag);

                return list;
            };
        }
    }
}
