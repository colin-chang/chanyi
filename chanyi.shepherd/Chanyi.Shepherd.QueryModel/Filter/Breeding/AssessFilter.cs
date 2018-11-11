using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Breeding
{
    public abstract class AssessFilter : BaseModelWithPrincipalFilter
    {
        public string SheepId { get; set; }

        public float? MaxWeight { get; set; }
        public float? MinWeight { get; set; }

        /// <summary>
        /// 体型评分
        /// </summary>
        public float? MaxHabitusScore { get; set; }
        public float? MinHabitusScore { get; set; }

        public DateTime? StartAssessDate { get; set; }
        public DateTime? EndAssessDate { get; set; }

        public GenderEnum? Gender { get; set; }

        /// <summary>
        /// 品种
        /// </summary>
        public string BreedId { get; set; }

        protected List<string> GetSqlWhereAndPms(out IDbParameters pms)
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

            if (MaxWeight != null)
            {
                list.Add("a.\"Weight\"<=@MaxWeight");
                pms.AddWithValue("MaxWeight", MaxWeight);
            }
            if (MinWeight != null)
            {
                list.Add("a.\"Weight\">=@MinWeight");
                pms.AddWithValue("MinWeight", MinWeight);
            }

            if (MaxHabitusScore != null)
            {
                list.Add("a.\"HabitusScore\"<=@MaxHabitusScore");
                pms.AddWithValue("MaxHabitusScore", MaxHabitusScore);
            }
            if (MinHabitusScore != null)
            {
                list.Add("a.\"HabitusScore\">=@MinHabitusScore");
                pms.AddWithValue("MinHabitusScore", MinHabitusScore);
            }

            if (StartAssessDate != null)
            {
                list.Add("a.\"AssessDate\">=@StartAssessDate");
                pms.AddWithValue("StartAssessDate", StartAssessDate);
            }
            if (EndAssessDate != null)
            {
                list.Add("a.\"AssessDate\"<=@EndAssessDate");
                pms.AddWithValue("EndAssessDate", EndAssessDate);
            }

            if (Gender != null)
            {
                list.Add("s.\"Gender\"=@Gender");
                pms.AddWithValue("Gender", (int)Gender);
            }

            if (!string.IsNullOrWhiteSpace(BreedId))
            {
                list.Add("b.\"Id\"=@BreedId");
                pms.AddWithValue("BreedId", BreedId);
            }

            return list;
        }
    }
}
