using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Filter
{
    [DataContract]
   public class BreedFilter
    {

       [DataMember]
       public string Name { get; set; }

       [DataMember]
       public string Description { get; set; }

        public string ToSqlWhere(out MySqlParameter[] paras)
        {
            if (FilterHelper.IsObjectEmpty<AccountFilter>(this))
            {
                paras = null;
                return string.Empty;
            }

            List<string> listWhere = new List<string>();
            List<MySqlParameter> listPara = new List<MySqlParameter>();

             if (!string.IsNullOrWhiteSpace(Name))
            {
                listWhere.Add("Name=@Name");
                listPara.Add(new MySqlParameter("Name", this.Name));

            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                listWhere.Add("Description=@Description");
                listPara.Add(new MySqlParameter("Description", this.Description));
            }
           

    }
}
