using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.Filter.System
{
    public class PermissionFilter : BaseModelFilter
    {
        public string Name { get; set; }

        public string Description { get; set; }

        //public string URL { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                #region 基本信息
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("p.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("p.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }
                if (StartCreateTime != null)
                {
                    list.Add("p.\"CreateTime\">=@StartCreateTime");
                    pms.AddWithValue("StartCreateTime", StartCreateTime);
                }
                if (EndCreateTime != null)
                {
                    list.Add("p.\"CreateTime\"<=@EndCreateTime");
                    pms.AddWithValue("EndCreateTime", EndCreateTime);
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("p.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                #endregion

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    list.Add("p.\"Name\" like @Name");
                    pms.AddWithValue("Name", Name.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(Description))
                {
                    list.Add("p.\"Description\" like @Description");
                    pms.AddWithValue("Description", Description.Wrap("%"));
                }

                //if (!string.IsNullOrWhiteSpace(Description))
                //{
                //    list.Add("p.\"URL\" like @URL");
                //    pms.AddWithValue("URL", URL.Wrap("%"));
                //}

                return list;
            };
        }
    }
}