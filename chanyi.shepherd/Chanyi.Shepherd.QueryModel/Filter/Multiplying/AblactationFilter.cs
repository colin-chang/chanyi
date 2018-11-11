using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Multiplying
{
    public class AblactationFilter : BaseModelWithPrincipalFilter
    {
        //
        public string SheepId { get; set; }

        public GenderEnum? Gender { get; set; }

        public DateTime? StartBirthday { get; set; }
        public DateTime? EndBirthday { get; set; }

        public float? MaxBirthWeight { get; set; }
        public float? MinBirthWeight { get; set; }

        public DateTime? StartAblactationDate { get; set; }
        public DateTime? EndAblactationDate { get; set; }

        public float? MaxAblactationWeight { get; set; }
        public float? MinAblactationWeight { get; set; }

        public int? MaxAblactationDays { get; set; }
        public int? MinAblactationDays { get; set; }

        //断奶
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
             {
                 List<string> list = new List<string>();
                 pms = Template.CreateDbParameters();

                 #region 基本的
                 if (!string.IsNullOrWhiteSpace(Id))
                 {
                     list.Add("a.\"Id\"=@Id");
                     pms.AddWithValue("Id", Id);
                 }
                 if (!string.IsNullOrWhiteSpace(OperatorId))
                 {
                     list.Add("a.\"OperatorId\"=@OperatorId");
                     pms.AddWithValue("OperatorId", OperatorId);
                 }
                 if (StartCreateTime != null)
                 {
                     list.Add("a.\"CreateTime\">=@StartCreateTime");
                     pms.AddWithValue("StartCreateTime", StartCreateTime);
                 }
                 if (EndCreateTime != null)
                 {
                     list.Add("a.\"CreateTime\"<=@EndCreateTime");
                     pms.AddWithValue("EndCreateTime", EndCreateTime);
                 }
                 if (!string.IsNullOrWhiteSpace(Remark))
                 {
                     list.Add("a.\"Remark\" like @Remark");
                     pms.AddWithValue("Remark", Remark.Wrap("%"));
                 }

                 if (!string.IsNullOrWhiteSpace(PrincipalId))
                 {
                     list.Add("a.\"PrincipalId\" = @PrincipalId");
                     pms.AddWithValue("PrincipalId", PrincipalId);
                 }
                 #endregion

                 if (!string.IsNullOrWhiteSpace(SheepId))
                 {
                     list.Add("a.\"SheepId\"=@SheepId");
                     pms.AddWithValue("SheepId", SheepId);
                 }

                 if (Gender != null)
                 {
                     list.Add("s.\"Gender\"=@Gender");
                     pms.AddWithValue("Gender", (int)Gender);
                 }


                 if (StartBirthday != null)
                 {
                     list.Add("s.\"Birthday\">=@StartBirthday");
                     pms.AddWithValue("StartBirthday", StartBirthday);
                 }
                 if (EndBirthday != null)
                 {
                     list.Add("s.\"Birthday\"<=@EndBirthday");
                     pms.AddWithValue("EndBirthday", EndBirthday);
                 }


                 if (MaxBirthWeight != null)
                 {
                     list.Add("s.\"BirthWeight\"<=@MaxBirthWeight");
                     pms.AddWithValue("MaxBirthWeight", MaxBirthWeight);
                 }
                 if (MinBirthWeight != null)
                 {
                     list.Add("s.\"BirthWeight\">=@MinBirthWeight");
                     pms.AddWithValue("MinBirthWeight", MinBirthWeight);
                 }


                 if (StartAblactationDate != null)
                 {
                     list.Add("s.\"AblactationDate\">=@StartAblactationDate");
                     pms.AddWithValue("StartAblactationDate", StartAblactationDate);
                 }
                 if (EndAblactationDate != null)
                 {
                     list.Add("s.\"AblactationDate\"<=@EndAblactationDate");
                     pms.AddWithValue("EndAblactationDate", EndAblactationDate);
                 }


                 if (MaxAblactationWeight != null)
                 {
                     list.Add("s.\"AblactationWeight\"<=@MaxAblactationWeight");
                     pms.AddWithValue("MaxAblactationWeight", MaxAblactationWeight);
                 }
                 if (MinAblactationWeight != null)
                 {
                     list.Add("s.\"AblactationWeight\">=@MinAblactationWeight");
                     pms.AddWithValue("MinAblactationWeight", MinAblactationWeight);
                 }


                 if (MaxAblactationDays != null)
                 {
                     list.Add("(s.\"AblactationDate\" - s.\"Birthday\")<=@MaxAblactationDays");
                     pms.AddWithValue("MaxAblactationDays", MaxAblactationDays);
                 }
                 if (MinAblactationDays != null)
                 {
                     list.Add("(s.\"AblactationDate\" - s.\"Birthday\")>=@MinAblactationDays");
                     pms.AddWithValue("MinAblactationDays", MinAblactationDays);
                 }

                 return list;
             };
        }
    }
}
