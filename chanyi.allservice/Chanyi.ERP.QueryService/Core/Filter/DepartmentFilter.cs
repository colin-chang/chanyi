using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using MySql.Data.MySqlClient;

using Chanyi.ERP.QueryService.Core.Helper;
using Chanyi.Utility.Common;

namespace Chanyi.ERP.QueryService.Core.Filter
{
    [DataContract]
    public class DepartmentFilter : BaseFilter
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]

        public string Name { get; set; }

        [DataMember]
        public string Desc { get; set; }

        [DataMember]
        public bool? IsEnabled { get; set; }

        [DataMember]
        public string CreaterName { get; set; }

        [DataMember]
        public DateTime? StartTime { get; set; }

        [DataMember]
        public DateTime? EndTime { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out List<MySqlParameter> ps) =>
            {
                List<string> listWhere = new List<string>();
                ps = new List<MySqlParameter>();

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    listWhere.Add("Id=@Id");
                    ps.Add(new MySqlParameter("Id", this.Id));
                }
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    listWhere.Add("Name like @Name");
                    ps.Add(new MySqlParameter("Name", this.Name.Wrap("%")));

                }
                if (!string.IsNullOrWhiteSpace(Desc))
                {
                    listWhere.Add("Desc=@Desc");
                    ps.Add(new MySqlParameter("Desc", this.Desc));
                }
                if (IsEnabled != null)
                {
                    listWhere.Add("IsEnabled=@IsEnabled");
                    ps.Add(new MySqlParameter("IsEnabled", this.IsEnabled == true ? 1 : 0));
                }
                if (!string.IsNullOrWhiteSpace(CreaterName))
                {
                    listWhere.Add("(u.UserName like @UserName or u.RealName like @UserName)");
                    ps.Add(new MySqlParameter("UserName", this.CreaterName.Wrap("%")));
                }
                if (StartTime != null)
                {
                    listWhere.Add("CreateTime >= @StartTime");
                    ps.Add(new MySqlParameter("StartTime", this.StartTime));
                }
                if (EndTime != null)
                {
                    listWhere.Add("CreateTime <= @EndTime");
                    ps.Add(new MySqlParameter("EndTime", this.EndTime));
                }

                return listWhere;
            };
        }
    }
}
