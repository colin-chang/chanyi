using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using Chanyi.ERP.QueryService.Core.Helper;
using Chanyi.Utility.Common;

namespace Chanyi.ERP.QueryService.Core.Filter
{
    [DataContract]
    public class BookkeepingFilter : BaseFilter
    {
        #region Property
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public DateTime? StartTime { get; set; }

        [DataMember]
        public DateTime? EndTime { get; set; }

        [DataMember]
        public string VoucherType { get; set; }

        [DataMember]
        public string VoucherNum { get; set; }

        [DataMember]
        public string Abstract { get; set; }

        [DataMember]
        public decimal? MaxAmount { get; set; }

        [DataMember]
        public decimal? MinAmount { get; set; }

        [DataMember]
        public int? Direction { get; set; }

        [DataMember]
        public decimal? MaxBalance { get; set; }

        [DataMember]
        public decimal? MinBalance { get; set; }

        [DataMember]
        public string CreaterName { get; set; }

        [DataMember]
        public DateTime? StartCreateTime { get; set; }

        [DataMember]
        public DateTime? EndCreateTime { get; set; }

        [DataMember]
        public string AbandonReason { get; set; }

        [DataMember]
        public int? Status { get; set; }

        [DataMember]
        public string Remark { get; set; }
        #endregion

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out List<MySqlParameter> ps) =>
            {
                List<string> listWhere = new List<string>();
                ps = new List<MySqlParameter>();

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    listWhere.Add("b.Id=@Id");
                    ps.Add(new MySqlParameter("Id", this.Id));
                }
                if (StartTime != null)
                {
                    listWhere.Add("Time >= @StartTime");
                    ps.Add(new MySqlParameter("StartTime", this.StartTime));
                }
                if (EndTime != null)
                {
                    listWhere.Add("Time <= @EndTime");
                    ps.Add(new MySqlParameter("EndTime", this.EndTime));
                }
                if (!string.IsNullOrWhiteSpace(VoucherType))
                {
                    listWhere.Add("VoucherType=@VoucherType");
                    ps.Add(new MySqlParameter("VoucherType", this.VoucherType));
                }
                if (!string.IsNullOrWhiteSpace(VoucherNum))
                {
                    listWhere.Add("b.VoucherNum=@VoucherNum");
                    ps.Add(new MySqlParameter("VoucherNum", this.VoucherNum));
                }
                if (!string.IsNullOrWhiteSpace(Abstract))
                {
                    listWhere.Add("b.Abstract=@Abstract");
                    ps.Add(new MySqlParameter("Abstract", this.Abstract));
                }
                if (MaxAmount != null)
                {
                    listWhere.Add("Amount <= @MaxAmount");
                    ps.Add(new MySqlParameter("MaxAmount", this.MaxAmount));
                }
                if (MinAmount != null)
                {
                    listWhere.Add("Amount >= @MinAmount");
                    ps.Add(new MySqlParameter("MinAmount", this.MinAmount));
                }
                if (Direction != null)
                {
                    listWhere.Add("Direction=@Direction");
                    ps.Add(new MySqlParameter("Direction", this.Direction));
                }
                if (MaxBalance != null)
                {
                    listWhere.Add("Balance <= @MaxBalance");
                    ps.Add(new MySqlParameter("MaxBalance", this.MaxBalance));
                }
                if (MinBalance != null)
                {
                    listWhere.Add("Balance >= @MinBalance");
                    ps.Add(new MySqlParameter("MinBalance", this.MinBalance));
                }
                if (!string.IsNullOrWhiteSpace(CreaterName))
                {
                    listWhere.Add("(u.UserName like @UserName or u.RealName like @UserName)");
                    ps.Add(new MySqlParameter("UserName", this.CreaterName.Wrap("%")));
                }
                if (StartCreateTime != null)
                {
                    listWhere.Add("b.CreateTime >= @StartCreateTime");
                    ps.Add(new MySqlParameter("StartCreateTime", this.StartCreateTime));
                }
                if (EndCreateTime != null)
                {
                    listWhere.Add("b.CreateTime <= @EndCreateTime");
                    ps.Add(new MySqlParameter("EndCreateTime", this.EndCreateTime));
                }
                if (!string.IsNullOrWhiteSpace(AbandonReason))
                {
                    listWhere.Add("b.AbandonReason=@AbandonReason");
                    ps.Add(new MySqlParameter("AbandonReason", this.AbandonReason));
                }
                if (Status != null)
                {
                    listWhere.Add("Status=@Status");
                    ps.Add(new MySqlParameter("Status", this.Status));
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    listWhere.Add("b.Remark=@Remark");
                    ps.Add(new MySqlParameter("Remark", this.Remark));
                }


                return listWhere;
            };
        }

    }
}
