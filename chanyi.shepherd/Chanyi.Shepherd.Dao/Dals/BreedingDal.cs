using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Spring.Data.Common;
using Spring.Transaction.Interceptor;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.Filter.Breeding;
using Chanyi.Shepherd.QueryModel.Model.Breeding;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region Breeding

        [Transaction]
        public void AddAssessStudsheep(string id, string studSheepId, float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Assess\" (\"Id\",\"SheepId\",\"Weight\",\"HabitusScore\",\"AssessDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:sheepId,:weight,:habitusScore,:assessDate,:principalId,:operatorId,:createTime,:remark);INSERT into \"T_AssessStudsheep\" (\"AssessId\",\"MatingAbility\")VALUES(:assessId,:matingAbility)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("sheepId", studSheepId);
            pms.AddWithValue("weight", weight);
            pms.AddWithValue("habitusScore", habitusScore);
            pms.AddWithValue("assessDate", assessDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);


            pms.AddWithValue("assessId", id);
            pms.AddWithValue("matingAbility", matingAbility);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public int GetStudsheepCount(string studSheepId)
        {
            string sql = "SELECT COUNT (\"Id\") FROM \"T_AssessStudsheep\" ass JOIN \"T_Assess\" A ON  ass.\"AssessId\"=A .\"Id\"  WHERE \"SheepId\" =:sheepId";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sheepId", studSheepId);
            return GetDataCount(sql, pms);
        }

        public int GetFirstAssessCount(string sheepId)
        {
            string sql = "SELECT COUNT (\"Id\") FROM \"T_FirstAssess\" fa JOIN \"T_Assess\" A ON  fa.\"AssessId\"=A .\"Id\"  WHERE \"SheepId\" =:sheepId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sheepId", sheepId);
            return GetDataCount(sql, pms);
        }

        [Transaction]
        public void AddFirstAssess(string id, string sheepId, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Assess\" (\"Id\",\"SheepId\",\"Weight\",\"HabitusScore\",\"AssessDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:sheepId,:weight,:habitusScore,:assessDate,:principalId,:operatorId,:createTime,:remark);INSERT into \"T_FirstAssess\" (\"AssessId\")VALUES(:assessId)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("sheepId", sheepId);
            pms.AddWithValue("weight", weight);
            pms.AddWithValue("habitusScore", habitusScore);
            pms.AddWithValue("assessDate", assessDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("assessId", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public int GetSecondAssess(string sheepId)
        {
            string sql = "SELECT COUNT (\"Id\") FROM \"T_SecondAssess\" sa JOIN \"T_Assess\" A ON  sa.\"AssessId\"=A .\"Id\"  WHERE \"SheepId\" =:sheepId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sheepId", sheepId);
            return GetDataCount(sql, pms);
        }

        [Transaction]
        public void AddSecondAssess(string id, string sheepId, float breedFeatureScore, float genitaliaScore, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Assess\" (\"Id\",\"SheepId\",\"Weight\",\"HabitusScore\",\"AssessDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:sheepId,:weight,:habitusScore,:assessDate,:principalId,:operatorId,:createTime,:remark);INSERT into \"T_SecondAssess\" (\"AssessId\",\"BreedFeatureScore\",\"GenitaliaScore\")VALUES(:assessId,:breedFeatureScore,:genitaliaScore);   UPDATE \"T_Sheep\" SET \"GrowthStage\"=:growthStage where \"Id\"=:updateSheepId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("sheepId", sheepId);
            pms.AddWithValue("weight", weight);
            pms.AddWithValue("habitusScore", habitusScore);
            pms.AddWithValue("assessDate", assessDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("assessId", id);
            pms.AddWithValue("breedFeatureScore", breedFeatureScore);
            pms.AddWithValue("genitaliaScore", genitaliaScore);

            pms.AddWithValue("growthStage", (int)GrowthStageEnum.TemporaryStudSheep);
            pms.AddWithValue("updateSheepId", sheepId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }


        public int GetThirdAssess(string sheepId)
        {
            string sql = "SELECT COUNT (\"Id\") FROM \"T_ThirdAssess\" sa JOIN \"T_Assess\" A ON  sa.\"AssessId\"=A .\"Id\"  WHERE \"SheepId\" =:sheepId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sheepId", sheepId);
            return GetDataCount(sql, pms);
        }

        [Transaction]
        public void AddThirdAssess(string id, string sheepId, float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Assess\" (\"Id\",\"SheepId\",\"Weight\",\"HabitusScore\",\"AssessDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:sheepId,:weight,:habitusScore,:assessDate,:principalId,:operatorId,:createTime,:remark);INSERT into \"T_ThirdAssess\" (\"AssessId\",\"MatingAbility\")VALUES(:assessId,:matingAbility)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("sheepId", sheepId);
            pms.AddWithValue("weight", weight);
            pms.AddWithValue("habitusScore", habitusScore);
            pms.AddWithValue("assessDate", assessDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("assessId", id);
            pms.AddWithValue("matingAbility", matingAbility);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        [Transaction]
        public void AddExceptAssessSheep(string id, string sheepId, string reason, GrowthStageEnum growthStageEnum, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_ExceptAssess\" (\"Id\",\"SheepId\",\"Reason\",\"SourceGrowthStage\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"IsDel\",\"Remark\")VALUES(:id,:sheepId,:reason,:sourceGrowthStage,:principalId,:operatorId,:createTime,false,:remark);update \"T_Sheep\" set \"GrowthStage\"=:growthStage where \"Id\"=:updateSheepId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("sheepId", sheepId);
            pms.AddWithValue("reason", reason);
            pms.AddWithValue("sourceGrowthStage", (int)growthStageEnum);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("growthStage", (int)GrowthStageEnum.FattingSheep);
            pms.AddWithValue("updateSheepId", sheepId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }
        [Transaction]
        public void DeleteExceptAssessSheep(string id)
        {
            string sql = "update \"T_ExceptAssess\" set \"IsDel\"=true  where \"Id\"=:id;update \"T_Sheep\" set \"GrowthStage\"=(SELECT \"SourceGrowthStage\" from \"T_ExceptAssess\" where \"Id\"=:eId) where \"Id\"=(SELECT \"SheepId\" from \"T_ExceptAssess\" where \"Id\"=:exId)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("eId", id);
            pms.AddWithValue("exId", id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public List<AssessStudsheep> GetAssessStudsheep(AssessStudsheepFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY a.\"AssessDate\" DESC) AS rownum,A .\"Id\", A .\"OperatorId\", s.\"SerialNumber\", ass.\"MatingAbility\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\",  s.\"Birthday\",s.\"Gender\", b.\"Name\" as \"BreedName\", A .\"CreateTime\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", A .\"Remark\" FROM \"T_AssessStudsheep\" ass JOIN \"T_Assess\" AS A ON A .\"Id\" = ass.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            string countSql = "SELECT count(A.\"Id\") FROM \"T_AssessStudsheep\" ass JOIN \"T_Assess\" AS A ON A .\"Id\" = ass.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            List<AssessStudsheep> list = GetPagedData<AssessStudsheep, AssessStudsheepFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);

            list.ForEach(a =>
            {
                //TODO:TotalScore
                a.TotalScore = ((a.HabitusScore + a.MatingAbility) * 1.0 / 2).ToString("0.00");
            });

            return list;
        }
        public List<AssessStudsheep> GetAssessStudsheep(AssessStudsheepFilter filter, int rowCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY a.\"AssessDate\" DESC) AS rownum,A .\"Id\", A .\"OperatorId\", s.\"SerialNumber\", ass.\"MatingAbility\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\",  s.\"Birthday\",s.\"Gender\", b.\"Name\" as \"BreedName\", A .\"CreateTime\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", A .\"Remark\" FROM \"T_AssessStudsheep\" ass JOIN \"T_Assess\" AS A ON A .\"Id\" = ass.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            List<AssessStudsheep> list = GetRuledRowsData<AssessStudsheep, AssessStudsheepFilter>(rowCount, querySql, filter);
            list.ForEach(a =>
            {
                //TODO:TotalScore
                a.TotalScore = ((a.HabitusScore + a.MatingAbility) * 1.0 / 2).ToString("0.00");
            });
            return list;
        }
        public AssessStudsheep GetAssessStudsheepById(string id)
        {
            string sql = "SELECT A .\"Id\", s.\"SerialNumber\", ass.\"MatingAbility\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\", s.\"Birthday\", s.\"Gender\", A .\"PrincipalId\", A .\"OperatorId\", A .\"CreateTime\", A .\"Remark\" FROM \"T_AssessStudsheep\" ass JOIN \"T_Assess\" AS A ON A .\"Id\" = ass.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" where A.\"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            List<AssessStudsheep> list = GetData<AssessStudsheep>(sql, pms);

            list.ForEach(a =>
            {
                //TODO:TotalScore
            });

            return list.FirstOrDefault();
        }

        public List<FirstAssess> GetFirstAssess(FirstAssessFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY a.\"AssessDate\" DESC) AS rownum,A .\"Id\", A .\"OperatorId\", s.\"SerialNumber\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\", s.\"Birthday\", s.\"Gender\", b.\"Name\" as \"BreedName\", A .\"CreateTime\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", A .\"Remark\" FROM \"T_FirstAssess\" fa JOIN \"T_Assess\" AS A ON A .\"Id\" = fa.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            string countSql = "SELECT count(A.\"Id\") FROM \"T_FirstAssess\" fa JOIN \"T_Assess\" AS A ON A .\"Id\" = fa.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            List<FirstAssess> list = GetPagedData<FirstAssess, FirstAssessFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);

            list.ForEach(a =>
            {
                //TODO:TotalScore
                a.TotalScore = a.HabitusScore.ToString("0.00");
            });

            return list;
        }
        public List<FirstAssess> GetFirstAssess(FirstAssessFilter filter, int rowCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY a.\"AssessDate\" DESC) AS rownum,A .\"Id\", A .\"OperatorId\", s.\"SerialNumber\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\", s.\"Birthday\", s.\"Gender\", b.\"Name\" as \"BreedName\", A .\"CreateTime\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", A .\"Remark\" FROM \"T_FirstAssess\" fa JOIN \"T_Assess\" AS A ON A .\"Id\" = fa.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            List<FirstAssess> list = GetRuledRowsData<FirstAssess, FirstAssessFilter>(rowCount, querySql, filter);
            list.ForEach(a =>
            {
                //TODO:TotalScore
                a.TotalScore = a.HabitusScore.ToString("0.00");
            });
            return list;
        }
        public FirstAssess GetFirstAssessById(string id)
        {
            string sql = "SELECT A .\"Id\", s.\"SerialNumber\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\", s.\"Birthday\", s.\"Gender\", A .\"PrincipalId\", A .\"OperatorId\", A .\"CreateTime\", A .\"Remark\" FROM \"T_FirstAssess\" fa JOIN \"T_Assess\" AS A ON A .\"Id\" = fa.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" where A.\"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            List<FirstAssess> list = GetData<FirstAssess>(sql, pms);

            list.ForEach(a =>
            {
                //TODO:TotalScore
            });

            return list.FirstOrDefault();
        }

        public List<SecondAssess> GetSecondAssess(SecondAssessFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY a.\"AssessDate\" DESC) AS rownum,A .\"Id\", A .\"OperatorId\", s.\"SerialNumber\", sa.\"BreedFeatureScore\", sa.\"GenitaliaScore\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\",  s.\"Birthday\",s.\"Gender\", b.\"Name\" as \"BreedName\", A .\"CreateTime\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", A .\"Remark\" FROM \"T_SecondAssess\" sa JOIN \"T_Assess\" AS A ON A .\"Id\" = sa.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            string countSql = "SELECT count(a.\"Id\")  FROM \"T_SecondAssess\" sa JOIN \"T_Assess\" AS A ON A .\"Id\" = sa.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            List<SecondAssess> list = GetPagedData<SecondAssess, SecondAssessFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);

            list.ForEach(a =>
            {
                //TODO:TotalScore
                a.TotalScore = ((a.BreedFeatureScore + a.GenitaliaScore + a.HabitusScore) * 1.0 / 3).ToString("0.00");
            });

            return list;
        }
        public List<SecondAssess> GetSecondAssess(SecondAssessFilter filter, int rowCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY a.\"AssessDate\" DESC) AS rownum,A .\"Id\", A .\"OperatorId\", s.\"SerialNumber\", sa.\"BreedFeatureScore\", sa.\"GenitaliaScore\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\",  s.\"Birthday\",s.\"Gender\", b.\"Name\" as \"BreedName\", A .\"CreateTime\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", A .\"Remark\" FROM \"T_SecondAssess\" sa JOIN \"T_Assess\" AS A ON A .\"Id\" = sa.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            List<SecondAssess> list = GetRuledRowsData<SecondAssess, SecondAssessFilter>(rowCount, querySql, filter);
            list.ForEach(a =>
            {
                //TODO:TotalScore
                a.TotalScore = ((a.BreedFeatureScore + a.GenitaliaScore + a.HabitusScore) * 1.0 / 3).ToString("0.00");
            });
            return list;
        }
        public SecondAssess GetSecondAssessById(string id)
        {
            string sql = "SELECT A .\"Id\", s.\"SerialNumber\", sa.\"BreedFeatureScore\", sa.\"GenitaliaScore\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\", s.\"Birthday\", s.\"Gender\", A .\"PrincipalId\", A .\"OperatorId\", A .\"CreateTime\", A .\"Remark\" FROM \"T_SecondAssess\" sa JOIN \"T_Assess\" AS A ON A .\"Id\" = sa.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" where A.\"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            List<SecondAssess> list = GetData<SecondAssess>(sql, pms);

            list.ForEach(a =>
            {
                //TODO:TotalScore
            });

            return list.FirstOrDefault();
        }

        public List<ThirdAssess> GetThirdAssess(ThirdAssessFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY a.\"AssessDate\" DESC) AS rownum,A .\"Id\", A .\"OperatorId\", ta.\"MatingAbility\", s.\"SerialNumber\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\", s.\"Birthday\", s.\"Gender\", b.\"Name\" as \"BreedName\", A .\"CreateTime\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", A .\"Remark\" FROM \"T_ThirdAssess\" ta JOIN \"T_Assess\" AS A ON A .\"Id\" = ta.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            string countSql = "SELECT count(a.\"Id\") FROM \"T_ThirdAssess\" ta JOIN \"T_Assess\" AS A ON A .\"Id\" = ta.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            List<ThirdAssess> list = GetPagedData<ThirdAssess, ThirdAssessFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);

            list.ForEach(a =>
            {
                //TODO:TotalScore
                a.TotalScore = ((a.MatingAbility + a.HabitusScore) * 1.0 / 2).ToString("0.00");
            });

            return list;
        }
        public List<ThirdAssess> GetThirdAssess(ThirdAssessFilter filter, int rowCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY a.\"AssessDate\" DESC) AS rownum,A .\"Id\", A .\"OperatorId\", ta.\"MatingAbility\", s.\"SerialNumber\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\", s.\"Birthday\", s.\"Gender\", b.\"Name\" as \"BreedName\", A .\"CreateTime\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", A .\"Remark\" FROM \"T_ThirdAssess\" ta JOIN \"T_Assess\" AS A ON A .\"Id\" = ta.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            List<ThirdAssess> list = GetRuledRowsData<ThirdAssess, ThirdAssessFilter>(rowCount, querySql, filter);
            list.ForEach(a =>
            {
                //TODO:TotalScore
                a.TotalScore = ((a.MatingAbility + a.HabitusScore) * 1.0 / 2).ToString("0.00");
            });
            return list;
        }
        public ThirdAssess GetThirdAssessById(string id)
        {
            string sql = "SELECT A .\"Id\", s.\"SerialNumber\", ta.\"MatingAbility\", A .\"Weight\", A .\"HabitusScore\", A .\"AssessDate\", s.\"Birthday\", s.\"Gender\", A .\"PrincipalId\", A .\"OperatorId\", A .\"CreateTime\", A .\"Remark\" FROM \"T_ThirdAssess\" ta JOIN \"T_Assess\" AS A ON A .\"Id\" = ta.\"AssessId\" JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" where A.\"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            List<ThirdAssess> list = GetData<ThirdAssess>(sql, pms);

            list.ForEach(a =>
            {
                //TODO:TotalScore
            });

            return list.FirstOrDefault();
        }

        public List<ExceptAssess> GetExceptAssess(ExceptAssessFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = "select \"row_number\" () OVER (ORDER BY ex.\"CreateTime\" DESC)  AS rownum,ex.\"Id\",s.\"SerialNumber\",ex.\"Reason\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",ex.\"Remark\",ex.\"CreateTime\" from \"T_ExceptAssess\" ex  join \"T_Sheep\" s on s.\"Id\"=ex.\"SheepId\" join \"T_Employee\" e on e.\"Id\"=ex.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=ex.\"OperatorId\"";

            string countSql = "select count(ex.\"Id\")  from \"T_ExceptAssess\" ex  join \"T_Sheep\" s on s.\"Id\"=ex.\"SheepId\" join \"T_Employee\" e on e.\"Id\"=ex.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=ex.\"OperatorId\"";

            return GetPagedData<ExceptAssess, ExceptAssessFilter>(pageIndex, pageSize, out totalCount, countSql, sql, filter);
        }
        public List<ExceptAssess> GetExceptAssess(ExceptAssessFilter filter, int rowCount)
        {
            string sql = "select \"row_number\" () OVER (ORDER BY ex.\"CreateTime\" DESC)  AS rownum,ex.\"Id\",s.\"SerialNumber\",ex.\"Reason\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",ex.\"Remark\",ex.\"CreateTime\"  from \"T_ExceptAssess\" ex  join \"T_Sheep\" s on s.\"Id\"=ex.\"SheepId\" join \"T_Employee\" e on e.\"Id\"=ex.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=ex.\"OperatorId\"";

            return GetRuledRowsData<ExceptAssess, ExceptAssessFilter>(rowCount, sql, filter);
        }

        [Transaction]
        public void UpdateAssessStudsheep(float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_AssessStudsheep\" SET \"MatingAbility\"=:matingAbility where \"AssessId\"=:assessId;UPDATE \"T_Assess\" SET \"Weight\"=:weight,\"HabitusScore\"=:habitusScore,\"AssessDate\"=:assessDate,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();

            pms.AddWithValue("matingAbility", matingAbility);
            pms.AddWithValue("assessId", id);

            pms.AddWithValue("weight", weight);
            pms.AddWithValue("habitusScore", habitusScore);
            pms.AddWithValue("assessDate", assessDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public void UpdateFirstAssess(float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_Assess\" SET \"Weight\"=:weight,\"HabitusScore\"=:habitusScore,\"AssessDate\"=:assessDate,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("weight", weight);
            pms.AddWithValue("habitusScore", habitusScore);
            pms.AddWithValue("assessDate", assessDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        [Transaction]
        public void UpdateSecondAssess(float breedFeatureScore, float genitaliaScore, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_SecondAssess\" SET \"BreedFeatureScore\"=:breedFeatureScore,\"GenitaliaScore\"=:genitaliaScore where \"AssessId\"=:assessId;UPDATE \"T_Assess\" SET \"Weight\"=:weight,\"HabitusScore\"=:habitusScore,\"AssessDate\"=:assessDate,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("breedFeatureScore", breedFeatureScore);
            pms.AddWithValue("genitaliaScore", genitaliaScore);
            pms.AddWithValue("assessId", id);

            pms.AddWithValue("weight", weight);
            pms.AddWithValue("habitusScore", habitusScore);
            pms.AddWithValue("assessDate", assessDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        [Transaction]
        public void UpdateThirdAssess(float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_ThirdAssess\" SET \"MatingAbility\"=:matingAbility where \"AssessId\"=:assessId;UPDATE \"T_Assess\" SET \"Weight\"=:weight,\"HabitusScore\"=:habitusScore,\"AssessDate\"=:assessDate,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("matingAbility", matingAbility);
            pms.AddWithValue("assessId", id);

            pms.AddWithValue("weight", weight);
            pms.AddWithValue("habitusScore", habitusScore);
            pms.AddWithValue("assessDate", assessDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        #endregion
    }
}
