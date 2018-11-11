using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using MySql.Data.MySqlClient;

using Chanyi.ERP.QueryService.Core.Helper;

namespace Chanyi.ERP.QueryService.Core.Filter
{
    [DataContract]
    public class UserFilter : BaseFilter
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string RealName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string IdNum { get; set; }

        [DataMember]
        public string PhoneNum { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Degree { get; set; }

        [DataMember]
        public DateTime ? StartEntryTime { get; set; }
        [DataMember]
        public DateTime? EndEntryTime { get; set; }

        [DataMember]
        public string Nation { get; set; }

        [DataMember]
        public DateTime? StartBirthday { get; set; }

        [DataMember]
        public DateTime? EndBirthday { get; set; }

        [DataMember]
        public string EmployeeNum { get; set; }

        [DataMember]
        public string CreaterName { get; set; }

        [DataMember]
        public string Status { get; set; }

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
                if (!string.IsNullOrWhiteSpace(UserName))
                {
                    listWhere.Add("UserName like @UserName");
                    ps.Add(new MySqlParameter("UserName", this.UserName));

                }
                if (!string.IsNullOrWhiteSpace(RealName))
                {
                    listWhere.Add("RealName like @RealName");
                    ps.Add(new MySqlParameter("RealName", this.RealName));

                }
                if (!string.IsNullOrWhiteSpace(Password))
                {
                    listWhere.Add("Password=@Password");
                    ps.Add(new MySqlParameter("Password", this.Password));
                }
                if (!string.IsNullOrWhiteSpace(IdNum))
                {
                    listWhere.Add("IdNum=@IdNum");
                    ps.Add(new MySqlParameter("IdNum", this.IdNum));
                }
                if (!string.IsNullOrWhiteSpace(PhoneNum))
                {
                    listWhere.Add("PhoneNum=@PhoneNum");
                    ps.Add(new MySqlParameter("PhoneNum", this.PhoneNum));
                }
                if (!string.IsNullOrWhiteSpace(Gender))
                {
                    listWhere.Add("Gender like @Gender");
                    ps.Add(new MySqlParameter("Gender", this.Gender));
                }
                if (!string.IsNullOrWhiteSpace(Address))
                {
                    listWhere.Add("(Address like @Address )");
                    ps.Add(new MySqlParameter("Address", this.Address));
                }
                if (!string.IsNullOrWhiteSpace(Degree))
                {
                    listWhere.Add("(Degree like @Degree )");
                    ps.Add(new MySqlParameter("Degree", this.Degree));
                }
                if (StartEntryTime != null)
                {
                    listWhere.Add("EntryTime >= @StartEntryTime");
                    ps.Add(new MySqlParameter("StartEntryTime", this.StartEntryTime));
                }
                if (EndEntryTime != null)
                {
                    listWhere.Add("EntryTime <= @EndEntryTime");
                    ps.Add(new MySqlParameter("EndEntryTime", this.EndEntryTime));
                }
                if (!string.IsNullOrWhiteSpace(Nation))
                {
                    listWhere.Add("(Nation like @Nation )");
                    ps.Add(new MySqlParameter("Nation", this.Nation));
                }
                if (StartBirthday != null)
                {
                    listWhere.Add("Birthday >= @StartBirthday");
                    ps.Add(new MySqlParameter("StartBirthday", this.StartBirthday));
                }
                if (EndBirthday != null)
                {
                    listWhere.Add("Birthday >= @EndBirthday");
                    ps.Add(new MySqlParameter("EndBirthday", this.EndBirthday));
                }
                if (!string.IsNullOrWhiteSpace(EmployeeNum))
                {
                    listWhere.Add("EmployeeNum=@EmployeeNum");
                    ps.Add(new MySqlParameter("EmployeeNum", this.EmployeeNum));
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

                if (!string.IsNullOrWhiteSpace(Status))
                {
                    listWhere.Add("Status like @Status");
                    ps.Add(new MySqlParameter("Status", this.Status));
                }

                return listWhere;
            };
        }
    }
}
