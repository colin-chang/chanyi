using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Filter
{
    [DataContract]
    public class DiseaseTypeFilter : IQueryFilter
    {
        [DataMember]
        public string TypeId { get; set; }

        [DataMember]
        public string Pid { get; set; }

          public string ToSqlWhere(out MySqlParameter[] paras)
        {
            if (FilterHelper.IsObjectEmpty<AccountFilter>(this))
            {
                paras = null;
                return string.Empty;
            }

            List<string> listWhere = new List<string>();
            List<MySqlParameter> listPara = new List<MySqlParameter>();

            if (!string.IsNullOrWhiteSpace(TypeId))
            {
                listWhere.Add("a.TypeId=@TypeId");
                listPara.Add(new MySqlParameter("TypeId", this.TypeId));
            }
            if (!string.IsNullOrWhiteSpace(Pid))
            {
                listWhere.Add("Pid=@Pid");
                listPara.Add(new MySqlParameter("Pid", this.Pid));

            }
 
    }
}

