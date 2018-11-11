using System;
using System.Collections.Generic;

using Spring.Data.Common;

using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.Filter.BaseInfo
{
    public class SheepFilter : BaseModelWithPrincipalFilter
    {
        #region 属性
        public string SerialNumber { get; set; }

        public GenderEnum? Gender { get; set; }

        public GrowthStageEnum? GrowthStage { get; set; }

        public string BreedId { get; set; }

        /// <summary>
        /// 羊圈
        /// </summary>
        public string SheepfoldId { get; set; }

        public float? MaxBirthWeight { get; set; }
        public float? MinBirthWeight { get; set; }

        /// <summary>
        /// 同胎羔羊数
        /// </summary>
        public int? CompatriotNumber { get; set; }
        //public int? MaxCompatriotNumber { get; set; }
        //public int? MinCompatriotNumber { get; set; }

        public DateTime? StartBirthday { get; set; }
        public DateTime? EndBirthday { get; set; }

        /// <summary>
        /// 断奶重
        /// </summary>
        public float? MaxAblactationWeight { get; set; }
        public float? MinAblactationWeight { get; set; }

        /// <summary>
        /// 断奶日期
        /// </summary>
        public DateTime? StartAblactationDate { get; set; }
        public DateTime? EndAblactationDate { get; set; }

        public OriginEnum? Origin { get; set; }

        public string FatherId { get; set; }

        public string MotherId { get; set; }

        public SheepStatusEnum? Status { get; set; }

        public string MotherSerialNumber { get; set; }

        public string FatherSerialNumber { get; set; }


        //public string BreedName { get; set; }

        //public string SheepfoldName { get; set; }

        #endregion
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                #region 基本信息
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("s.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("s.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }
                //if (!string.IsNullOrWhiteSpace(OperatorName))
                //{
                //    list.Add("u.\"UserName\" like @OperatorName");
                //    pms.AddWithValue("OperatorName", OperatorName.Wrap("%"));
                //}
                if (StartCreateTime != null)
                {
                    list.Add("s.\"CreateTime\">=@StartCreateTime");
                    pms.AddWithValue("StartCreateTime", StartCreateTime);
                }
                if (EndCreateTime != null)
                {
                    list.Add("s.\"CreateTime\"<=@EndCreateTime");
                    pms.AddWithValue("EndCreateTime", EndCreateTime);
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("s.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(PrincipalId))
                {
                    list.Add("s.\"PrincipalId\" = @PrincipalId");
                    pms.AddWithValue("PrincipalId", PrincipalId);
                }

                //if (!string.IsNullOrWhiteSpace(PrincipalName))
                //{
                //    list.Add("e.\"Name\" like @PrincipalName");
                //    pms.AddWithValue("PrincipalName", PrincipalName.Wrap("%"));
                //}
                #endregion

                if (!string.IsNullOrWhiteSpace(SerialNumber))
                {
                    //list.Add("s.\"SerialNumber\"~*@SerialNumber");
                    list.Add("\"lower\"(s.\"SerialNumber\")=\"lower\"(@SerialNumber)");
                    pms.AddWithValue("SerialNumber", SerialNumber);
                }

                if (Gender != null)
                {
                    list.Add("s.\"Gender\"=@Gender");
                    pms.AddWithValue("Gender", (int)Gender);
                }

                if (GrowthStage != null)
                {
                    list.Add("s.\"GrowthStage\"=@GrowthStage");
                    pms.AddWithValue("GrowthStage", (int)GrowthStage);
                }


                if (!string.IsNullOrWhiteSpace(BreedId))
                {
                    list.Add("s.\"BreedId\"=@BreedId");
                    pms.AddWithValue("BreedId", BreedId);
                }

                if (!string.IsNullOrWhiteSpace(SheepfoldId))
                {
                    list.Add("s.\"SheepfoldId\"=@SheepfoldId");
                    pms.AddWithValue("SheepfoldId", SheepfoldId);
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

                if (CompatriotNumber != null)
                {
                    list.Add("s.\"CompatriotNumber\"=@CompatriotNumber");
                    pms.AddWithValue("CompatriotNumber", (int)CompatriotNumber);
                }

                //if (MaxCompatriotNumber != null)
                //{
                //    list.Add("s.\"CompatriotNumber\"<=@MaxCompatriotNumber");
                //    pms.AddWithValue("MaxCompatriotNumber", MaxCompatriotNumber);
                //}
                //if (MinCompatriotNumber != null)
                //{
                //    list.Add("s.\"CompatriotNumber\">=@MinCompatriotNumber");
                //    pms.AddWithValue("MinCompatriotNumber", MinCompatriotNumber);
                //}

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

                if (Origin != null)
                {
                    list.Add("s.\"Origin\"=@Origin");
                    pms.AddWithValue("Origin", (int)Origin);
                }

                if (!string.IsNullOrWhiteSpace(FatherId))
                {
                    list.Add("s.\"FatherId\"=@FatherId");
                    pms.AddWithValue("FatherId", FatherId);
                }

                if (!string.IsNullOrWhiteSpace(MotherId))
                {
                    list.Add("s.\"MotherId\"=@MotherId");
                    pms.AddWithValue("MotherId", MotherId);
                }

                if (Status != null)
                {
                    list.Add("s.\"Status\"=@Status");
                    pms.AddWithValue("Status", (int)Status);
                }
                else
                    list.Add("s.\"Status\"!=" + (int)SheepStatusEnum.Outer);

                if (!string.IsNullOrWhiteSpace(MotherSerialNumber))
                {
                    list.Add("sm.\"Id\" like @MotherSerialNumber");
                    pms.AddWithValue("MotherSerialNumber", MotherSerialNumber.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(FatherSerialNumber))
                {
                    list.Add("sf.\"Id\" like @FatherSerialNumber");
                    pms.AddWithValue("FatherSerialNumber", FatherSerialNumber.Wrap("%"));
                }

                //if (!string.IsNullOrWhiteSpace(BreedName))
                //{
                //    list.Add("b.\"Name\" like @BreedName");
                //    pms.AddWithValue("BreedName", BreedName.Wrap("%"));
                //}

                //if (!string.IsNullOrWhiteSpace(SheepfoldName))
                //{
                //    list.Add("f.\"Name\" like @SheepfoldName");
                //    pms.AddWithValue("SheepfoldName", SheepfoldName.Wrap("%"));
                //}


                return list;
            };
        }

        public override string ToSqlWhere(out IDbParameters pms)
        {
            List<string> listWhere = new List<string>();
            listWhere.AddRange(CreateGenerator()(out pms));

            return " where " + string.Join(" and ", listWhere);
        }
    }
}
