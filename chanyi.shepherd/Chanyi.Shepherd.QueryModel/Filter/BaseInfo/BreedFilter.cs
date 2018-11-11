using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.BaseInfo
{
    /// <summary>
    /// 品种
    /// </summary>
    public class BreedFilter : BaseModelFilter
    {
        public string Name { get; set; }

        public string Description { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                #region 基本信息
                 if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("b.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("b.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }
                //if (!string.IsNullOrWhiteSpace(OperatorName))
                //{
                //    list.Add("u.\"UserName\" like @OperatorName");
                //    pms.AddWithValue("OperatorName", OperatorName.Wrap("%"));
                //}
                if (StartCreateTime != null)
                {
                    list.Add("b.\"CreateTime\">=@StartCreateTime");
                    pms.AddWithValue("StartCreateTime", StartCreateTime);
                }
                if (EndCreateTime != null)
                {
                    list.Add("b.\"CreateTime\"<=@EndCreateTime");
                    pms.AddWithValue("EndCreateTime", EndCreateTime);
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("b.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                #endregion

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    list.Add("b.\"Name\" like @Name");
                    pms.AddWithValue("Name", Name.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(Description))
                {
                    list.Add("b.\"Description\" like @Description");
                    pms.AddWithValue("Description", Description.Wrap("%"));
                }

                return list;
            };
        }
    }
}
