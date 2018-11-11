using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using MySql.Data.MySqlClient;

using Chanyi.Utility.Common;
using Chanyi.ERP.QueryService.Core.Helper;


namespace Chanyi.ERP.QueryService.Core.Filter
{
    [DataContract]
    public class LogFilter : BaseFilter
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Operation { get; set; }

        [DataMember]
        public string OperatorName { get; set; }

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
                if (!string.IsNullOrWhiteSpace(Operation))
                {
                    listWhere.Add("Operation like @Operation");
                    ps.Add(new MySqlParameter("Operation", this.Operation.Wrap("%")));

                }
                if (!string.IsNullOrWhiteSpace(OperatorName))
                {
                    listWhere.Add("OperatorName like @OperatorName");
                    ps.Add(new MySqlParameter("OperatorName", this.OperatorName.Wrap("%")));
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
