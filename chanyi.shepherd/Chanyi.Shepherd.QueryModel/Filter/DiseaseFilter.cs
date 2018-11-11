using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using Spring.Data.Common;
using Spring.Data.Generic;
using MySql.Data.MySqlClient;

using Chanyi.ERP.QueryService.Core.Helper;

using Chanyi.Utility.Common;

 

namespace Chanyi.Shepherd.QueryModel.Filter
{
    [DataContract]
    public class DiseaseFilter : IQueryFilter
    {
        [DataMember]
        public string SicknessId { get; set; }

        public string ToSqlWhere(out MySqlParameter[] paras)
        {
            if (FilterHelper.IsObjectEmpty<DiseaseFilter>(this))
            {
                paras = null;
                return string.Empty;
            }

            List<string> listWhere = new List<string>();
            List<MySqlParameter> listPara = new List<MySqlParameter>();

            if (!string.IsNullOrWhiteSpace(Id))
            {
                listWhere.Add("a.SicknessId=@SicknessId");
                listPara.Add(new MySqlParameter("SicknessId", this.SicknessId));
            }
    }
}
