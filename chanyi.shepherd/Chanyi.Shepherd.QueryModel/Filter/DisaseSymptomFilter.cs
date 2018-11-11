using Spring.Context;
using Spring.Context.Support;
using Spring.Data.Common;
using Spring.Data.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Filter
{
    [DataContract]
   public class DisaseSymptomFilter
    {
        [DataMember]
        public string Id { get; set; }

         [DataMember]
        public string DiseaseId { get; set; }

         [DataMember]
        public string SymptomId { get; set; }

         public string ToSqlWhere(out MySqlParameter[] paras)
        {
            if (FilterHelper.IsObjectEmpty<AccountFilter>(this))
            {
                paras = null;
                return string.Empty;
            }

            List<string> listWhere = new List<string>();
            List<MySqlParameter> listPara = new List<MySqlParameter>();

            if (!string.IsNullOrWhiteSpace(Id))
            {
                listWhere.Add("a.Id=@Id");
                listPara.Add(new MySqlParameter("Id", this.Id));
            }
            if (!string.IsNullOrWhiteSpace(DisaseId))
            {
                listWhere.Add("a.DisaseId=@DiseaseId");
                listPara.Add(new MySqlParameter("DiseaseId", this.DiseaseId));

            }
            if (!string.IsNullOrWhiteSpace(SymptomId))
            {
                listWhere.Add("a.SymptomId=@SymptomId");
                listPara.Add(new MySqlParameter("SymptomId", this.SymptomId));
            }
    }
}
