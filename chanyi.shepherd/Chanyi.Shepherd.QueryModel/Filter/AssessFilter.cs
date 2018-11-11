using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Filter
{
    [DataContract]
   public class AssessFilter
    {
        [DataMember]
        public float Weight { get; set; }

        [DataMember]
        public string SheepId { get; set; }

        /// <summary>
        /// 体型评分
        /// </summary>
        [DataMember]
        public float HabitusScore { get; set; }

        [DataMember]
        public DateTime AssessDate { get; set; }

          public string ToSqlWhere(out MySqlParameter[] paras)
        {
            if (FilterHelper.IsObjectEmpty<AccountFilter>(this))
            {
                paras = null;
                return string.Empty;
            }

            List<string> listWhere = new List<string>();
            List<MySqlParameter> listPara = new List<MySqlParameter>();

              if (!string.IsNullOrWhiteSpace(Weight))
            {
                listWhere.Add("Weight=@Weight");
                listPara.Add(new MySqlParameter("Weight", this.Weight));
              }
              if (!string.IsNullOrWhiteSpace(SheepId))
            {
                listWhere.Add("SheepId=@SheepId");
                listPara.Add(new MySqlParameter("SheepId", this.SheepId));

            }
           


    }
}
