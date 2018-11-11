using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Filter
{
   [DataContract]
    public class SheepFilter : IQueryFilter
    {
      [DataMember]
       public string SerialNumber { get; set; }

      [DataMember]
       public GrowthStageEnum GrowthStage { get; set; }

      [DataMember]
       public GenderEnum Gender { get; set; }

      [DataMember]
       public string SheepfoldId { get; set; }

      [DataMember]
       public float BirthWeight { get; set; }

      [DataMember]
       public DateTime? StartBirthday { get; set; }

       [DataMember]
      public DateTime? EndBirthday { get; set; }
        /// <summary>
        /// 断奶重
        /// </summary>
      [DataMember]
        public float AblactationWeight { get; set; }

      [DataMember]
        public DateTime? startTime { get; set; }

      [DataMember]
    public DateTime?     EndTime {get;set;}

      [DataMember]
        public OriginEnum Origin { get; set; }

      [DataMember]
        public string FatherId { get; set; }

      [DataMember]
        public string MotherId { get; set; }

      [DataMember]
        public SheepStatusEnum Status { get; set; }

      public string ToSqlWhere(out MySqlParameter[] paras)
      {
          if (FilterHelper.IsObjectEmpty<AccountFilter>(this))
          {
              paras = null;
              return string.Empty;
          }

          List<string> listWhere = new List<string>();
          List<MySqlParameter> listPara = new List<MySqlParameter>();

          if (!string.IsNullOrWhiteSpace(SerialNumber))
          {
              listWhere.Add("SerialNumber=@SerialNumber");
              listPara.Add(new MySqlParameter("SerialNumber", this.SerialNumber));
          }
          if (!string.IsNullOrWhiteSpace(GrowthStage))
          {
              listWhere.Add("GrowthStage=@GrowthStage");
              listPara.Add(new MySqlParameter("GrowthStage", this.GrowthStage));

          }
          if (!string.IsNullOrWhiteSpace(Gender))
          {
              listWhere.Add("Gender=@Gender");
              listPara.Add(new MySqlParameter("Gender", this.Gender));
          }

          if (!string.IsNullOrWhiteSpace(SheepfoldId))
          {
              listWhere.Add("SheepfoldId=@SheepfoldId");
              listPara.Add(new MySqlParameter("SheepfoldId", this.SheepfoldId));
          }
          if (!string.IsNullOrWhiteSpace( BirthWeight))
          {
              listWhere.Add("( BirthWeight like @ BirthWeight or  BirthWeight like @ BirthWeight)");
              listPara.Add(new MySqlParameter(" BirthWeight", this. BirthWeight.Wrap("%")));
          }
          if (StartTime != null)
          {
              listWhere.Add("Birthday >= @StartTime");
              listPara.Add(new MySqlParameter("StartTime", this.StartTime));
          }
          if (EndTime != null)
          {
              listWhere.Add("Birthday <= @EndTime");
              listPara.Add(new MySqlParameter("EndTime", this.EndTime));
          }

          if (!string.IsNullOrWhiteSpace(AblactationWeight))
          {
              listWhere.Add("( AblactationWeight like @ AblactationWeight or  AblactationWeight like @ AblactationWeight)");
              listPara.Add(new MySqlParameter(" AblactationWeight", this.AblactationWeight.Wrap("%")));
          }

          if (StartTime != null)
          {
              listWhere.Add("AblactationDate >= @StartTime");
              listPara.Add(new MySqlParameter("StartTime", this.StartTime));
          }
          if (EndTime != null)
          {
              listWhere.Add("AblactationDate <= @EndTime");
              listPara.Add(new MySqlParameter("EndTime", this.EndTime));
          }

          if (!string.IsNullOrWhiteSpace(Origin))
          {
              listWhere.Add("(Origin like @Origin or Origin like @Origin)");
              listPara.Add(new MySqlParameter("Origin", this.Origin.Wrap("%")));
          }

          if (!string.IsNullOrWhiteSpace(FatherId))
          {
              listWhere.Add("FatherId=@FatherId");
              listPara.Add(new MySqlParameter("FatherId", this.FatherId));
          }

          if (!string.IsNullOrWhiteSpace(FatherId))
          {
              listWhere.Add("MotherId=@MotherId");
              listPara.Add(new MySqlParameter("MotherId", this.MotherId));
          }
          paras = listPara.ToArray();
          return " where " + string.Join(" and ", listWhere);
      }

    }
}
