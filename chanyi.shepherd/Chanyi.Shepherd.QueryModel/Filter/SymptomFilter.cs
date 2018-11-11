using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Filter
{
    [DataContract]
    public class SymptomFilter : IQueryFilter
    {
        [DataMember]
         public string SicknessId { get; set; }
        
        public string ToSqlWhere(out MySqlParameter[] paras)
        {
            if (FilterHelper.IsObjectEmpty<AccountFilter>(this))
            {
                paras = null;
                return string.Empty;
            }

            List<string> listWhere = new List<string>();
            List<MySqlParameter> listPara = new List<MySqlParameter>();

            if (!string.IsNullOrWhiteSpace(SicknessId))
            {
                listWhere.Add("a.SicknessId=@SicknessId");
                listPara.Add(new MySqlParameter("SicknessId", this.SicknessId));
    }
}
