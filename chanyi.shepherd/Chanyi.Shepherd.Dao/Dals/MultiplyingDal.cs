using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Spring.Data.Common;
using Spring.Data.Generic;
using Spring.Transaction.Interceptor;

using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using Chanyi.Shepherd.QueryModel.Filter.Multiplying;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.Dao.Mappers;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region Multiplying

        [Transaction]
        public void AddMating(string id, string femaleId, string maleId, DateTime matingDate, bool isRemindful, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "update \"T_Mating\" SET \"IsRemindful\"=FALSE where \"FemaleId\"=@oldFemaleId;INSERT into \"T_Mating\" (\"Id\",\"FemaleId\",\"MaleId\",\"MatingDate\",\"IsRemindful\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:femaleId,:maleId,:matingDate,:isRemindful,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("oldFemaleId", femaleId);

            pms.AddWithValue("id", id);
            pms.AddWithValue("femaleId", femaleId);
            pms.AddWithValue("maleId", maleId);
            pms.AddWithValue("matingDate", matingDate);
            pms.AddWithValue("isRemindful", isRemindful);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void UpdateMatingDisRemindful(string maleId)
        {
            string sql = "update \"T_Mating\" SET \"IsRemindful\"=FALSE where \"MaleId\"=@maleId;";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("maleId", maleId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public List<Mating> GetMating(MatingFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY M.\"MatingDate\" desc) \"rownum\",M.\"Id\",M.\"FemaleId\",M.\"MaleId\",M.\"MatingDate\",M.\"IsRemindful\",M.\"PrincipalId\",M.\"OperatorId\",M.\"CreateTime\",M.\"Remark\", sf.\"SerialNumber\" AS \"FemaleSerialNumber\", sm.\"SerialNumber\" AS \"MaleSerialNumber\", bf.\"Name\" AS \"FemaleBreed\", bm.\"Name\" AS \"MaleBreed\", u.\"UserName\" AS \"OperatorName\", e.\"Name\" AS \"PrincipalName\" FROM \"T_Mating\" M JOIN \"T_Sheep\" sm ON sm.\"Id\" =M.\"MaleId\" JOIN \"T_Sheep\" sf ON sf.\"Id\" =M.\"FemaleId\" JOIN \"T_Breed\" bf ON bf.\"Id\" = sf.\"BreedId\" JOIN \"T_Breed\" bm ON bm.\"Id\" = sm.\"BreedId\" JOIN \"T_User\" u ON u.\"Id\" =M.\"OperatorId\" JOIN \"T_Employee\" e ON e.\"Id\" =M.\"PrincipalId\"";


            string countSql = "SELECT COUNT (m.\"Id\") FROM \"T_Mating\" M JOIN \"T_Sheep\" sm ON sm.\"Id\" =M.\"MaleId\" JOIN \"T_Sheep\" sf ON sf.\"Id\" =M.\"FemaleId\" JOIN \"T_Breed\" bf ON bf.\"Id\" = sf.\"BreedId\" JOIN \"T_Breed\" bm ON bm.\"Id\" = sm.\"BreedId\" JOIN \"T_User\" u ON u.\"Id\" =M.\"OperatorId\" JOIN \"T_Employee\" e ON e.\"Id\" =M.\"PrincipalId\"";

            List<Mating> list = GetPagedData<Mating, MatingFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
            //EDC预产期
            string getEDCSql = "select \"Value\" from \"T_SheepParameter\" sp JOIN \"T_Settings\" s on s.\"Id\"=sp.\"SettingsId\" where s.\"Type\"=:type";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("type", (int)SettingsEnum.PreDeliveryRemaindful);
            int edcDays = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, getEDCSql, pms));

            list.ForEach(m => { m.EDC = m.MatingDate.AddDays(edcDays); });

            return list;
        }

        public List<Mating> GetMating(MatingFilter filter, int rowsCount)
        {
            string sql = "SELECT \"row_number\" () OVER (ORDER BY M.\"MatingDate\" desc) \"rownum\",M.\"Id\",M.\"FemaleId\",M.\"MaleId\",M.\"MatingDate\",M.\"IsRemindful\",M.\"PrincipalId\",M.\"OperatorId\",M.\"CreateTime\",M.\"Remark\", sf.\"SerialNumber\" AS \"FemaleSerialNumber\", sm.\"SerialNumber\" AS \"MaleSerialNumber\", bf.\"Name\" AS \"FemaleBreed\", bm.\"Name\" AS \"MaleBreed\", u.\"UserName\" AS \"OperatorName\", e.\"Name\" AS \"PrincipalName\" FROM \"T_Mating\" M JOIN \"T_Sheep\" sm ON sm.\"Id\" =M.\"MaleId\" JOIN \"T_Sheep\" sf ON sf.\"Id\" =M.\"FemaleId\" JOIN \"T_Breed\" bf ON bf.\"Id\" = sf.\"BreedId\" JOIN \"T_Breed\" bm ON bm.\"Id\" = sm.\"BreedId\" JOIN \"T_User\" u ON u.\"Id\" =M.\"OperatorId\" JOIN \"T_Employee\" e ON e.\"Id\" =M.\"PrincipalId\"";

            return GetRuledRowsData<Mating, MatingFilter>(rowsCount, sql, filter);
        }

        public Mating GetMatingById(string id)
        {
            string sql = "select m.\"Id\",m.\"FemaleId\",m.\"MaleId\",m.\"MatingDate\",m.\"IsRemindful\",m.\"PrincipalId\",m.\"OperatorId\",m.\"CreateTime\",m.\"Remark\",m.\"IsDel\",sm.\"SerialNumber\" as \"MaleSerialNumber\",sf.\"SerialNumber\" as \"FemaleSerialNumber\" from \"T_Mating\" as m join \"T_Sheep\" sm on m.\"MaleId\"=sm.\"Id\" join \"T_Sheep\" sf on m.\"FemaleId\"=sf.\"Id\" where m.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            return GetData<Mating>(sql, pms).FirstOrDefault();
        }

        public void DeleteMating(string id)
        {
            string sql = "update \"T_Mating\" set \"IsDel\"=true where \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms).ToString();
        }

        public List<MatingCount> GetMatingCount(string sheepId, int? count, DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize, out int totalCount)
        {
            List<MatingCount> list = new List<MatingCount>();

            StringBuilder sb = new StringBuilder();
            StringBuilder sbCount = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();


            sb.Append("SELECT 	\"row_number\" () OVER (ORDER BY \"SerialNumber\") \"rownum\",\"SerialNumber\",\"BreedName\",\"SheepfoldName\",\"Status\",COUNT (\"MaleId\") AS \"Count\" FROM(SELECT s.\"SerialNumber\",M .\"MaleId\",sf.\"Name\" AS \"SheepfoldName\",b.\"Name\" AS \"BreedName\",s.\"Status\"FROM \"T_Sheep\" s  JOIN \"T_Mating\" M ON M .\"FemaleId\" = s.\"Id\" JOIN \"T_Sheepfold\" sf ON sf.\"Id\" = s.\"SheepfoldId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" WHERE s.\"Gender\" =:gender");
            sbCount.Append("SELECT \"count\" (\"SerialNumber\") FROM ( SELECT \"SerialNumber\" FROM ( SELECT s.\"SerialNumber\" FROM \"T_Sheep\" s  JOIN \"T_Mating\" M ON M .\"FemaleId\" = s.\"Id\" JOIN \"T_Sheepfold\" sf ON sf.\"Id\" = s.\"SheepfoldId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" WHERE s.\"Gender\" =:gender  and M .\"IsDel\" = FALSE");

            pms.AddWithValue("gender", (int)GenderEnum.Female);
            if (startDate != null || endDate != null)
            {
                sb.Append(" and M .\"IsDel\" = FALSE  and m.\"MatingDate\">=:startDate and m.\"MatingDate\"<=:endDate ");
                sbCount.Append("  and m.\"MatingDate\">=:startDate and m.\"MatingDate\"<=:endDate ");
                pms.AddWithValue("startDate", startDate);
                pms.AddWithValue("endDate", endDate);
            }
            if (!string.IsNullOrEmpty(sheepId))
            {
                sb.Append(" and s.\"Id\"=:sheepId ");
                sbCount.Append(" and s.\"Id\"=:sheepId ");
                pms.AddWithValue("sheepId", sheepId);
            }

            sb.Append(")T GROUP BY \"SerialNumber\",\"BreedName\",\"SheepfoldName\",\"Status\" ");
            sbCount.Append(" )T GROUP BY \"SerialNumber\"");

            if (count != null)
            {
                sb.Append(" HAVING COUNT (\"MaleId\") =:cnt ");
                sbCount.Append(" HAVING COUNT (\"SerialNumber\") =:cnt ");
                pms.AddWithValue("cnt", count);
            }
            sb.Append("ORDER BY \"Count\" DESC ");
            sbCount.Append(" ) C");


            pageSize = pageSize < 1 ? 20 : pageSize;

            totalCount = GetDataCount(sbCount.ToString(), pms);

            int totalPages = (int)Math.Ceiling(totalCount * 1.0 / pageSize);
            totalPages = totalPages < 1 ? 1 : totalPages;
            pageIndex = pageIndex < 1 ? 1 : (pageIndex > totalPages ? totalPages : pageIndex);

            string sql = string.Format("select * from ({0}) as sr where sr.\"rownum\" BETWEEN {1} and {2}", sb.ToString(), (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);

            list = AdoTemplate.QueryWithRowMapper<MatingCount>(CommandType.Text, sql, new BaseRowMapper<MatingCount>(), pms).ToList();
            return list;

            //string sql = "SELECT \"row_number\" () OVER (ORDER BY \"SerialNumber\") \"rownum\",\"SerialNumber\", \"BreedName\", \"SheepfoldName\", \"Status\", COUNT (\"MaleId\") AS \"Count\" FROM ( SELECT s.\"SerialNumber\", M .\"MaleId\", sf.\"Name\" AS \"SheepfoldName\", b.\"Name\" AS \"BreedName\", s.\"Status\" FROM \"T_Sheep\" s LEFT JOIN \"T_Mating\" M ON M .\"FemaleId\" = s.\"Id\" AND M .\"IsDel\" = FALSE JOIN \"T_Sheepfold\" sf ON sf.\"Id\" = s.\"SheepfoldId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" WHERE s.\"Gender\" = 1 ) T GROUP BY \"SerialNumber\", \"BreedName\", \"SheepfoldName\", \"Status\" ORDER BY \"Count\" DESC";

            //string countSql = "SELECT \"count\" (\"SerialNumber\") FROM ( SELECT \"SerialNumber\" FROM ( SELECT s.\"SerialNumber\" FROM \"T_Sheep\" s LEFT JOIN \"T_Mating\" M ON M .\"FemaleId\" = s.\"Id\" AND M .\"IsDel\" = FALSE JOIN \"T_Sheepfold\" sf ON sf.\"Id\" = s.\"SheepfoldId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" WHERE s.\"Gender\" = 1 ) T GROUP BY \"SerialNumber\" ) C";

            //return GetPagedData<MatingCount, MatingCountFilter>(pageIndex, pageSize, out totalCount, countSql, sql, null);
        }
        public List<MatingCount> GetMatingCount(string sheepId, int? count, DateTime? startDate, DateTime? endDate, int rowsCount)
        {
            //string sql = "SELECT \"row_number\" () OVER (ORDER BY \"SerialNumber\") \"rownum\",\"SerialNumber\", \"BreedName\", \"SheepfoldName\", \"Status\", COUNT (\"MaleId\") AS \"Count\" FROM ( SELECT s.\"SerialNumber\", M .\"MaleId\", sf.\"Name\" AS \"SheepfoldName\", b.\"Name\" AS \"BreedName\", s.\"Status\" FROM \"T_Sheep\" s LEFT JOIN \"T_Mating\" M ON M .\"FemaleId\" = s.\"Id\" AND M .\"IsDel\" = FALSE JOIN \"T_Sheepfold\" sf ON sf.\"Id\" = s.\"SheepfoldId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" WHERE s.\"Gender\" = 1 ) T GROUP BY \"SerialNumber\", \"BreedName\", \"SheepfoldName\", \"Status\" ORDER BY \"Count\" DESC";

            //return GetRuledRowsData<MatingCount, MatingCountFilter>(rowsCount, sql, filter);

            List<MatingCount> list = new List<MatingCount>();

            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();

            sb.Append("SELECT 	\"row_number\" () OVER (ORDER BY \"SerialNumber\") \"rownum\",\"SerialNumber\",\"BreedName\",\"SheepfoldName\",\"Status\",COUNT (\"MaleId\") AS \"Count\" FROM(SELECT s.\"SerialNumber\",M .\"MaleId\",sf.\"Name\" AS \"SheepfoldName\",b.\"Name\" AS \"BreedName\",s.\"Status\"FROM \"T_Sheep\" s  JOIN \"T_Mating\" M ON M .\"FemaleId\" = s.\"Id\" JOIN \"T_Sheepfold\" sf ON sf.\"Id\" = s.\"SheepfoldId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" WHERE s.\"Gender\" =:gender");
            pms.AddWithValue("gender", (int)GenderEnum.Female);
            if (startDate != null || endDate != null)
            {
                sb.Append(" and M .\"IsDel\" = FALSE  and m.\"MatingDate\">=:startDate and m.\"MatingDate\"<=:endDate ");
                pms.AddWithValue("startDate", startDate);
                pms.AddWithValue("endDate", endDate);
            }
            if (!string.IsNullOrEmpty(sheepId))
            {
                sb.Append(" and s.\"Id\"=:sheepId ");
                pms.AddWithValue("sheepId", sheepId);
            }

            sb.Append(")T GROUP BY \"SerialNumber\",\"BreedName\",\"SheepfoldName\",\"Status\" ");

            if (count != null)
            {
                sb.Append("HAVING COUNT (\"MaleId\") =:cnt ");
                pms.AddWithValue("cnt", count);
            }
            sb.Append("ORDER BY \"Count\" DESC ");

            string sql = string.Format("select * from ({0}) as sr where sr.\"rownum\"<={1}", sb.ToString(), rowsCount);

            list = AdoTemplate.QueryWithRowMapper<MatingCount>(CommandType.Text, sql, new BaseRowMapper<MatingCount>(), pms).ToList();

            return list;
        }

        public Mating GetLatestMatingByFemaleId(string sheepId)
        {
            string sql = "select m.\"MatingDate\" from \"T_Mating\" as m where m.\"FemaleId\"=:sheepId and \"IsDel\"=false ORDER BY \"MatingDate\"";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sheepId", sheepId);

            return GetData<Mating>(sql, pms).FirstOrDefault();
        }

        public object GetLatestAbortionDateBySheepId(string sheepId)
        {
            string sql = "SELECT MAX(\"AbortionDate\") from \"T_Abortion\" where \"SheepId\"=:sheepId  and \"IsDel\"=false";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sheepId", sheepId);

            return AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
        }

        public object GetlatestDeliveryDateBySheepId(string sheepId)
        {
            string sql = "select MAX(\"DeliveryDate\") from \"T_Delivery\" where \"SheepId\"=:sheepId  and \"IsDel\"=false";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sheepId", sheepId);

            return AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
        }

        [Transaction]
        public void AddAbortion(string id, string sheepId, string reason, string dispose, DateTime abortionDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Abortion\" (\"Id\",\"SheepId\",\"Reason\",\"Dispose\",\"AbortionDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:sheepId,:reason,:dispose,:abortionDate,:principalId,:operatorId,:createTime,:remark);update \"T_Mating\" set \"IsRemindful\"=:isRemindful where \"FemaleId\"=:mid";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("sheepId", sheepId);
            pms.AddWithValue("reason", reason);
            pms.AddWithValue("dispose", dispose);
            pms.AddWithValue("abortionDate", abortionDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("isRemindful", false);
            pms.AddWithValue("mid", sheepId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        [Transaction]
        public void AddDelivery(string id, string femaleId, DeliveryWayEnum deliveryWay, MidwiferyReasonEnum deliverReason, string deliverReasonOtherDetail, int? liveMaleCount, int? liveFemaleCount, int totalCount, DateTime deliveryDate, string principalId, string operatorId, DateTime createTime, string remark)
        {

            string sql = "INSERT into \"T_Delivery\" (\"Id\",\"SheepId\",\"DeliveryWay\",\"DeliverReason\",\"DeliverReasonOtherDetail\",\"LiveMaleCount\",\"LiveFemaleCount\",\"TotalCount\",\"DeliveryDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:sheepId,:deliveryWay,:deliverReason,:deliverReasonOtherDetail,:liveMaleCount,:liveFemaleCount,:totalCount,:deliveryDate,:principalId,:operatorId,:createTime,:remark);update \"T_Mating\" set \"IsRemindful\"=:isRemindful where \"FemaleId\"=:mid;";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("sheepId", femaleId);
            pms.AddWithValue("deliveryWay", (int)deliveryWay);
            pms.AddWithValue("deliverReason", (int)deliverReason);
            pms.AddWithValue("deliverReasonOtherDetail", deliverReasonOtherDetail);
            pms.AddWithValue("liveMaleCount", liveMaleCount);
            pms.AddWithValue("liveFemaleCount", liveFemaleCount);

            pms.AddWithValue("totalCount", totalCount);
            pms.AddWithValue("deliveryDate", deliveryDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("isRemindful", false);
            pms.AddWithValue("mid", femaleId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }
        [Transaction]
        public void AddDelivery(string id, string femaleId, DeliveryWayEnum deliveryWay, MidwiferyReasonEnum deliverReason, string deliverReasonOtherDetail, int? liveMaleCount, int? liveFemaleCount, int totalCount, DateTime deliveryDate, string principalId, string operatorId, DateTime createTime, string remark, List<Chanyi.Shepherd.QueryModel.AddModel.BaseInfo.Sheep> lambList, string lambBreedId, string fatherId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT into \"T_Delivery\" (\"Id\",\"SheepId\",\"DeliveryWay\",\"DeliverReason\",\"DeliverReasonOtherDetail\",\"LiveMaleCount\",\"LiveFemaleCount\",\"TotalCount\",\"DeliveryDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:sheepId,:deliveryWay,:deliverReason,:deliverReasonOtherDetail,:liveMaleCount,:liveFemaleCount,:totalCount,:deliveryDate,:principalId,:operatorId,:createTime,:remark);update \"T_Mating\" set \"IsRemindful\"=:isRemindful where \"FemaleId\"=:mid;");

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("sheepId", femaleId);
            pms.AddWithValue("deliveryWay", (int)deliveryWay);
            pms.AddWithValue("deliverReason", (int)deliverReason);
            pms.AddWithValue("deliverReasonOtherDetail", deliverReasonOtherDetail);
            pms.AddWithValue("liveMaleCount", liveMaleCount);
            pms.AddWithValue("liveFemaleCount", liveFemaleCount);

            pms.AddWithValue("totalCount", totalCount);
            pms.AddWithValue("deliveryDate", deliveryDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("isRemindful", false);
            pms.AddWithValue("mid", femaleId);

            int mc = liveMaleCount ?? 0;
            int fc = liveFemaleCount ?? 0;

            for (int i = 0; i < lambList.Count; i++)
            {
                sb.AppendFormat("INSERT into \"T_Sheep\" (\"Id\",\"SerialNumber\",\"BreedId\",\"Gender\",\"GrowthStage\",\"SheepfoldId\",\"BirthWeight\",\"CompatriotNumber\",\"Birthday\",\"AblactationWeight\",\"AblactationDate\",\"Origin\",\"FatherId\",\"MotherId\",\"Status\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id{0},:serialNumber{0},:breedId{0},:gender{0},:growthStage{0},:sheepfoldId{0},:birthWeight{0},:compatriotNumber{0},:birthday{0},:ablactationWeight{0},:ablactationDate{0},:origin{0},:fatherId{0},:motherId{0},:status{0},:principalId{0},:operatorId{0},:createTime{0},:remark{0});", i);

                Chanyi.Shepherd.QueryModel.AddModel.BaseInfo.Sheep sheep = lambList[i];

                pms.AddWithValue("id" + i, Guid.NewGuid().ToString());
                pms.AddWithValue("serialNumber" + i, sheep.SerialNumber);
                pms.AddWithValue("breedId" + i, lambBreedId);
                pms.AddWithValue("gender" + i, (int)sheep.Gender);
                pms.AddWithValue("growthStage" + i, (int)GrowthStageEnum.Lamb);
                pms.AddWithValue("sheepfoldId" + i, sheep.SheepfoldId);
                pms.AddWithValue("birthWeight" + i, sheep.BirthWeight);

                pms.AddWithValue("compatriotNumber" + i, mc + fc);
                pms.AddWithValue("birthday" + i, deliveryDate);
                pms.AddWithValue("ablactationWeight" + i, null);
                pms.AddWithValue("ablactationDate" + i, null);
                pms.AddWithValue("origin" + i, (int)OriginEnum.HomeBred);
                pms.AddWithValue("fatherId" + i, fatherId);
                pms.AddWithValue("motherId" + i, femaleId);
                pms.AddWithValue("status" + i, (int)SheepStatusEnum.Nomal);
                pms.AddWithValue("principalId" + i, principalId);
                pms.AddWithValue("operatorId" + i, operatorId);
                pms.AddWithValue("createTime" + i, createTime);
                pms.AddWithValue("remark" + i, sheep.Remark);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }


        public List<Sheep> GetMatingBreedInfoByFemaleId(string femaleId)
        {
            string sql = "select s.\"Id\",s.\"BreedId\",b.\"Name\" as \"BreedName\",s.\"Gender\" from \"T_Sheep\" s join \"T_Breed\" b on b.\"Id\"=s.\"BreedId\" where s.\"Id\"=(select \"MaleId\" from \"T_Mating\" where \"FemaleId\"=:femaleId and \"IsDel\"=FALSE ORDER BY \"MatingDate\" desc LIMIT 1) or s.\"Id\"=:id;";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("femaleId", femaleId);
            pms.AddWithValue("id", femaleId);
            return GetData<Sheep>(sql, pms);
        }

        [Transaction]
        public void AddAblactation(string id, string sheepId, float ablactationWeight, DateTime ablactationDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Ablactation\" (\"Id\",\"SheepId\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:sheepId,:principalId,:operatorId,:createTime,:remark);update \"T_Sheep\" set \"AblactationWeight\"=:ablactationWeight,\"AblactationDate\"=:ablactationDate where \"Id\"=:sheepId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("sheepId", sheepId);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("ablactationWeight", ablactationWeight);
            pms.AddWithValue("ablactationDate", ablactationDate);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public int GetAblactation(string sheepId)
        {
            string sql = "select count(\"Id\") from \"T_Ablactation\" where \"SheepId\"=:sheepId";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sheepId", sheepId);
            return GetDataCount(sql, pms);
        }

        public List<Delivery> GetDelivery(DeliveryFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY d.\"CreateTime\" DESC) AS rownum, d.\"Id\", ds.\"SerialNumber\" AS \"FemaleNumber\", d.\"DeliveryWay\", d.\"DeliverReason\", d.\"DeliverReasonOtherDetail\", d.\"DeliveryDate\", d.\"LiveMaleCount\", d.\"LiveFemaleCount\", d.\"TotalCount\", M .\"MatingDate\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", d.\"CreateTime\", d.\"Remark\" FROM \"T_Delivery\" d JOIN \"T_Mating\" M ON M .\"FemaleId\" = d.\"SheepId\" AND M .\"MatingDate\" IN ( SELECT \"max\" (mt.\"MatingDate\") FROM \"T_Mating\" mt WHERE mt.\"IsDel\" = FALSE AND mt.\"FemaleId\" = d.\"SheepId\" GROUP BY mt.\"FemaleId\" HAVING MAX (mt.\"MatingDate\") < d.\"DeliveryDate\" ) JOIN \"T_Sheep\" ds ON ds.\"Id\" = d.\"SheepId\" JOIN \"T_Employee\" e ON e.\"Id\" = d.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = d.\"OperatorId\"";

            string countSql = "SELECT count(d.\"Id\") FROM \"T_Delivery\" d JOIN \"T_Mating\" M ON M .\"FemaleId\" = d.\"SheepId\" AND M .\"MatingDate\" IN ( SELECT \"max\" (mt.\"MatingDate\") FROM \"T_Mating\" mt WHERE mt.\"IsDel\" = FALSE AND mt.\"FemaleId\" = d.\"SheepId\" GROUP BY mt.\"FemaleId\" HAVING MAX (mt.\"MatingDate\") < d.\"DeliveryDate\" ) JOIN \"T_Sheep\" ds ON ds.\"Id\" = d.\"SheepId\" JOIN \"T_Employee\" e ON e.\"Id\" = d.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = d.\"OperatorId\"";

            return GetPagedData<Delivery, DeliveryFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);

        }

        public List<Delivery> GetDelivery(DeliveryFilter filter, int rowsCount)
        {
            string sql = "SELECT \"row_number\" () OVER (ORDER BY d.\"CreateTime\" DESC) AS rownum, d.\"Id\", ds.\"SerialNumber\" AS \"FemaleNumber\", d.\"DeliveryWay\", d.\"DeliverReason\", d.\"DeliverReasonOtherDetail\", d.\"DeliveryDate\", d.\"LiveMaleCount\", d.\"LiveFemaleCount\", d.\"TotalCount\", M .\"MatingDate\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", d.\"CreateTime\", d.\"Remark\" FROM \"T_Delivery\" d JOIN \"T_Mating\" M ON M .\"FemaleId\" = d.\"SheepId\" AND M .\"MatingDate\" IN ( SELECT \"max\" (mt.\"MatingDate\") FROM \"T_Mating\" mt WHERE mt.\"IsDel\" = FALSE AND mt.\"FemaleId\" = d.\"SheepId\" GROUP BY mt.\"FemaleId\" HAVING MAX (mt.\"MatingDate\") < d.\"DeliveryDate\" ) JOIN \"T_Sheep\" ds ON ds.\"Id\" = d.\"SheepId\" JOIN \"T_Employee\" e ON e.\"Id\" = d.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = d.\"OperatorId\"";

            return GetRuledRowsData<Delivery, DeliveryFilter>(rowsCount, sql, filter);
        }

        public Delivery GetDeliveryById(string id)
        {
            string sql = "select d.\"Id\",d.\"SheepId\",s.\"SerialNumber\" as \"FemaleNumber\",d.\"DeliveryWay\",d.\"DeliverReason\",d.\"DeliverReasonOtherDetail\",d.\"DeliveryDate\",d.\"LiveMaleCount\",d.\"LiveFemaleCount\",d.\"TotalCount\",d.\"PrincipalId\",d.\"OperatorId\",d.\"CreateTime\",d.\"Remark\",d.\"IsDel\" from \"T_Delivery\" as d join \"T_Sheep\" s on s.\"Id\"=d.\"SheepId\" where d.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            return GetData<Delivery>(sql, pms).FirstOrDefault();
        }
        public void DeleteDelivery(string id)
        {
            string sql = "update \"T_Delivery\" set \"IsDel\"=true where \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms).ToString();
        }

        public List<Abortion> GetAboration(AbortionFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY A .\"CreateTime\" DESC) AS rownum, A .\"Id\", fs.\"SerialNumber\" AS \"FemaleNumber\", ms.\"SerialNumber\" AS \"MaleNumber\", A .\"Reason\", A .\"Dispose\", A .\"AbortionDate\", A .\"CreateTime\", M .\"MatingDate\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\",  A .\"Remark\" FROM \"T_Abortion\" A JOIN \"T_Mating\" M ON M .\"FemaleId\" = A .\"SheepId\" AND M .\"MatingDate\" IN ( SELECT \"max\" (mt.\"MatingDate\") FROM \"T_Mating\" mt where mt.\"IsDel\"=FALSE and mt.\"FemaleId\"=A.\"SheepId\" GROUP BY mt.\"FemaleId\" HAVING max(mt.\"MatingDate\")<a.\"AbortionDate\") JOIN \"T_Sheep\" fs ON fs.\"Id\" = A .\"SheepId\" JOIN \"T_Sheep\" ms ON ms.\"Id\" = M .\"MaleId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            string countSql = "SELECT count(a.\"Id\") FROM \"T_Abortion\" A JOIN \"T_Mating\" M ON M .\"FemaleId\" = A .\"SheepId\" AND M .\"MatingDate\" IN ( SELECT \"max\" (mt.\"MatingDate\") FROM \"T_Mating\" mt where mt.\"IsDel\"=FALSE  and mt.\"FemaleId\"=A.\"SheepId\"  GROUP BY mt.\"FemaleId\" HAVING max(mt.\"MatingDate\")<a.\"AbortionDate\") JOIN \"T_Sheep\" fs ON fs.\"Id\" = A .\"SheepId\" JOIN \"T_Sheep\" ms ON ms.\"Id\" = M .\"MaleId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            List<Abortion> list = GetPagedData<Abortion, AbortionFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);

            //list.ForEach(t =>
            //{
            //    //( A .\"AbortionDate\" - M .\"MatingDate\" ) AS \"AbortionDays\",
            //    //t.AbortionDays = t.AbortionDate == null || t.MatingDate == null || t.AbortionDate < t.MatingDate ? 0 : (t.AbortionDate - t.MatingDate).Days;
            //});

            return list;
        }

        public List<Abortion> GetAboration(AbortionFilter filter, int rowsCount)
        {
            string sql = "SELECT \"row_number\" () OVER (ORDER BY A .\"CreateTime\" DESC) AS rownum, A .\"Id\", fs.\"SerialNumber\" AS \"FemaleNumber\", ms.\"SerialNumber\" AS \"MaleNumber\", A .\"Reason\", A .\"Dispose\", A .\"AbortionDate\", A .\"CreateTime\", M .\"MatingDate\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\",  A .\"Remark\" FROM \"T_Abortion\" A JOIN \"T_Mating\" M ON M .\"FemaleId\" = A .\"SheepId\" AND M .\"MatingDate\" IN ( SELECT \"max\" (mt.\"MatingDate\") FROM \"T_Mating\" mt where mt.\"IsDel\"=FALSE  and mt.\"FemaleId\"=A.\"SheepId\"  GROUP BY mt.\"FemaleId\" ) JOIN \"T_Sheep\" fs ON fs.\"Id\" = A .\"SheepId\" JOIN \"T_Sheep\" ms ON ms.\"Id\" = M .\"MaleId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";
            List<Abortion> list = GetRuledRowsData<Abortion, AbortionFilter>(rowsCount, sql, filter);

            //list.ForEach(t =>
            //{
            //    t.AbortionDays = t.AbortionDate == null || t.MatingDate == null || t.AbortionDate < t.MatingDate ? 0 : (t.AbortionDate - t.MatingDate).Days;
            //});

            return list;
        }
        public Abortion GetAbortionById(string id)
        {
            string sql = "select a.\"Id\",a.\"SheepId\",s.\"SerialNumber\" as \"FemaleNumber\",a.\"Reason\",a.\"Dispose\",a.\"AbortionDate\",a.\"PrincipalId\",a.\"OperatorId\",a.\"CreateTime\",a.\"Remark\",a.\"IsDel\" from \"T_Abortion\" as a join \"T_Sheep\" s on s.\"Id\"=a.\"SheepId\" where a.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            return GetData<Abortion>(sql, pms).FirstOrDefault();
        }

        public void DeleteAbortion(string id)
        {
            string sql = "update \"T_Abortion\" set \"IsDel\"=true where \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms).ToString();
        }

        public List<Ablactation> GetAblactation(AblactationFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT  \"row_number\" () OVER (ORDER BY s.\"AblactationDate\" DESC) AS rownum, A .\"Id\", s.\"SerialNumber\", e.\"Name\" AS \"PrincipalName\", s.\"Gender\", s.\"Birthday\", s.\"BirthWeight\", s.\"AblactationWeight\", s.\"AblactationDate\", u.\"UserName\" AS \"OperatorName\", A .\"CreateTime\", A .\"Remark\" FROM \"T_Ablactation\" A JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            string countSql = "SELECT count(a.\"Id\") FROM \"T_Ablactation\" A JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            return GetPagedData<Ablactation, AblactationFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<Ablactation> GetAblactation(AblactationFilter filter, int rowsCount)
        {
            string querySql = "SELECT  \"row_number\" () OVER (ORDER BY s.\"AblactationDate\" DESC) AS rownum, A .\"Id\", s.\"SerialNumber\", e.\"Name\" AS \"PrincipalName\", s.\"Gender\", s.\"Birthday\", s.\"BirthWeight\", s.\"AblactationWeight\", s.\"AblactationDate\", u.\"UserName\" AS \"OperatorName\", A .\"CreateTime\", A .\"Remark\" FROM \"T_Ablactation\" A JOIN \"T_Sheep\" s ON s.\"Id\" = A .\"SheepId\" JOIN \"T_Employee\" e ON e.\"Id\" = A .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = A .\"OperatorId\"";

            return GetRuledRowsData<Ablactation, AblactationFilter>(rowsCount, querySql, filter);
        }
        public Ablactation GetAblactationById(string id)
        {
            string sql = "select a.\"Id\",a.\"SheepId\",s.\"SerialNumber\",a.\"PrincipalId\",s.\"AblactationDate\",s.\"AblactationWeight\",a.\"OperatorId\",a.\"CreateTime\",a.\"Remark\" from \"T_Ablactation\" as a join \"T_Sheep\" s on s.\"Id\"=a.\"SheepId\" where a.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            return GetData<Ablactation>(sql, pms).FirstOrDefault();
        }

        public void UpdateMating(string femaleId, string maleId, DateTime matingDate, bool isRemindful, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_Mating\" SET \"FemaleId\"=:femaleId,\"MaleId\"=:maleId,\"MatingDate\"=:matingDate,\"IsRemindful\"=:isRemindful,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("femaleId", femaleId);
            pms.AddWithValue("maleId", maleId);
            pms.AddWithValue("matingDate", matingDate);
            pms.AddWithValue("isRemindful", isRemindful);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        [Transaction]
        public void UpdateAblactation(float ablactationWeight, DateTime ablactationDate, string sheepId, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_Sheep\" SET \"AblactationWeight\"=:ablactationWeight,\"AblactationDate\"=:ablactationDate where \"Id\"=:sheepId;UPDATE \"T_Ablactation\" SET \"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("ablactationWeight", ablactationWeight);
            pms.AddWithValue("ablactationDate", ablactationDate);
            pms.AddWithValue("sheepId", sheepId);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public void UpdateAbortion(string reason, string dispose, DateTime abortionDate, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_Abortion\" SET \"Reason\"=:reason,\"Dispose\"=:dispose,\"AbortionDate\"=:abortionDate,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("reason", reason);
            pms.AddWithValue("dispose", dispose);
            pms.AddWithValue("abortionDate", abortionDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void UpdateDelivery(DeliveryWayEnum deliveryWay, MidwiferyReasonEnum deliverReason, string deliverReasonOtherDetail, int? liveMaleCount, int? liveFemaleCount, int totalCount, DateTime deliveryDate, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_Delivery\" SET \"DeliveryWay\"=:deliveryWay,\"DeliverReason\"=:deliverReason,\"DeliverReasonOtherDetail\"=:deliverReasonOtherDetail,\"LiveMaleCount\"=:liveMaleCount,\"LiveFemaleCount\"=:liveFemaleCount,\"TotalCount\"=:totalCount,\"DeliveryDate\"=:deliveryDate,\"PrincipalId\"=:principalId,\"Remark\"=:remark  where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("deliveryWay", (int)deliveryWay);
            pms.AddWithValue("deliverReason", (int)deliverReason);
            pms.AddWithValue("deliverReasonOtherDetail", deliverReasonOtherDetail);
            pms.AddWithValue("liveMaleCount", liveMaleCount);
            pms.AddWithValue("liveFemaleCount", liveFemaleCount);
            pms.AddWithValue("totalCount", totalCount);
            pms.AddWithValue("deliveryDate", deliveryDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        #endregion
    }
}
