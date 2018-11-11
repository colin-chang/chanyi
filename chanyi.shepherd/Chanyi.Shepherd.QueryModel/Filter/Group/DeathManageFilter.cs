using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Group
{
    public class DeathManageFilter : BaseModelWithPrincipalFilter
    {
        public string SheepId { get; set; }

        public GenderEnum? Gender { get; set; }

        public string BreedId { get; set; }

        public GrowthStageEnum? GrowthStage { get; set; }

        //public string SheepfoldId { get; set; }

        public DeathDisposeEnum? Dispose { get; set; }

        public DateTime? StartDeathDate { get; set; }
        public DateTime? EndDeathDate { get; set; }

        public bool IsDel { get { return false; } }


        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                #region 基本的

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("d.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("d.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }
                if (StartCreateTime != null)
                {
                    list.Add("d.\"CreateTime\">=@StartCreateTime");
                    pms.AddWithValue("StartCreateTime", StartCreateTime);
                }
                if (EndCreateTime != null)
                {
                    list.Add("d.\"CreateTime\"<=@EndCreateTime");
                    pms.AddWithValue("EndCreateTime", EndCreateTime);
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("d.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(PrincipalId))
                {
                    list.Add("d.\"PrincipalId\" = @PrincipalId");
                    pms.AddWithValue("PrincipalId", PrincipalId);
                }

                #endregion


                if (!string.IsNullOrWhiteSpace(SheepId))
                {
                    list.Add("d.\"SheepId\"=@SheepId");
                    pms.AddWithValue("SheepId", SheepId);
                }

                if (Gender != null)
                {
                    list.Add("s.\"Gender\"=@Gender");
                    pms.AddWithValue("Gender", (int)Gender);
                }

                if (!string.IsNullOrWhiteSpace(BreedId))
                {
                    list.Add("s.\"BreedId\"=@BreedId");
                    pms.AddWithValue("BreedId", BreedId);
                }

                if (GrowthStage != null)
                {
                    list.Add("s.\"GrowthStage\"=@GrowthStage");
                    pms.AddWithValue("GrowthStage", (int)GrowthStage);
                }

                //if (!string.IsNullOrWhiteSpace(SheepfoldId))
                //{
                //    list.Add("s.\"SheepfoldId\"=@SheepfoldId");
                //    pms.AddWithValue("SheepfoldId", SheepfoldId);
                //}

                if (Dispose != null)
                {
                    list.Add("d.\"Dispose\"=@Dispose");
                    pms.AddWithValue("Dispose", (int)Dispose);
                }

                if (StartDeathDate != null)
                {
                    list.Add("d.\"DeathDate\">=@StartDeathDate");
                    pms.AddWithValue("StartDeathDate", StartDeathDate);
                }
                if (EndDeathDate != null)
                {
                    list.Add("d.\"DeathDate\"<=@EndDeathDate");
                    pms.AddWithValue("EndDeathDate", EndDeathDate);
                }

                list.Add("\"IsDel\"=@IsDel");
                pms.AddWithValue("IsDel", IsDel);

                return list;
            };
        }
    }
}
