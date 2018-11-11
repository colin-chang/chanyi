using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Spring.Data.Common;
using Spring.Transaction.Interceptor;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.Filter.BaseInfo;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region BaseInfo

        public List<Breed> GetBreed(BreedFilter filter)
        {
            string sql = "SELECT b.\"Id\",b.\"Name\",b.\"Description\",b.\"PrincipalId\",b.\"OperatorId\",b.\"CreateTime\",b.\"Remark\",u.\"UserName\" AS \"OperatorName\", e.\"Name\" AS \"PrincipalName\" FROM \"T_Breed\" b join \"T_User\" u ON u.\"Id\" = b.\"OperatorId\" join \"T_Employee\" e ON e.\"Id\" = b.\"PrincipalId\"";
            return GetData<Breed, BreedFilter>(sql, filter);
        }
        public List<Breed> GetBreed(BreedFilter filter, int rowsCount)
        {
            string sql = "SELECT \"row_number\"() OVER(ORDER BY  e.\"Name\") \"rownum\",b.\"Id\",b.\"Name\",b.\"Description\",b.\"PrincipalId\",b.\"OperatorId\",b.\"CreateTime\",b.\"Remark\",u.\"UserName\" AS \"OperatorName\", e.\"Name\" AS \"PrincipalName\" FROM \"T_Breed\" b join \"T_User\" u ON u.\"Id\" = b.\"OperatorId\" join \"T_Employee\" e ON e.\"Id\" = b.\"PrincipalId\"";

            return GetRuledRowsData<Breed, BreedFilter>(rowsCount, sql, filter);
        }

        public List<Sheep> GetSheep(SheepFilter filter, int rowsCount)
        {
            string sql = "SELECT \"row_number\"() OVER(ORDER BY s.\"CreateTime\" desc) \"rownum\",s.\"Id\", s.\"SerialNumber\",s.\"Gender\", s.\"GrowthStage\",s.\"BirthWeight\", s.\"CompatriotNumber\", s.\"Birthday\", s.\"AblactationWeight\", s.\"AblactationDate\", s.\"Origin\",s.\"Status\",s.\"CreateTime\", s.\"Remark\", sf.\"SerialNumber\" AS \"FatherSerialNumber\", sm.\"SerialNumber\" AS \"MotherSerialNumber\", b.\"Name\" AS \"BreedName\", f.\"Name\" AS \"SheepfoldName\", u.\"UserName\" AS \"OperatorName\", e.\"Name\" AS \"PrincipalName\" FROM \"T_Sheep\" s left join \"T_Sheep\" sf ON sf.\"Id\" = s.\"FatherId\" left join \"T_Sheep\" sm ON sm.\"Id\" = s.\"MotherId\" join \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" join \"T_Sheepfold\" f ON f.\"Id\" = s.\"SheepfoldId\" and  f.\"SysFlag\"=FALSE join \"T_User\" u ON u.\"Id\" = s.\"OperatorId\" join \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\"";

            return GetRuledRowsData<Sheep, SheepFilter>(rowsCount, sql, filter);
        }

        public List<Sheep> GetSheep(SheepFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\"() OVER(ORDER BY s.\"CreateTime\" desc) \"rownum\",s.\"Id\", s.\"SerialNumber\",s.\"Gender\", s.\"GrowthStage\",s.\"BirthWeight\", s.\"CompatriotNumber\", s.\"Birthday\", s.\"AblactationWeight\", s.\"AblactationDate\", s.\"Origin\",s.\"Status\",s.\"CreateTime\", s.\"Remark\", sf.\"SerialNumber\" AS \"FatherSerialNumber\", sm.\"SerialNumber\" AS \"MotherSerialNumber\", b.\"Name\" AS \"BreedName\", f.\"Name\" AS \"SheepfoldName\", u.\"UserName\" AS \"OperatorName\", e.\"Name\" AS \"PrincipalName\" FROM \"T_Sheep\" s left join \"T_Sheep\" sf ON sf.\"Id\" = s.\"FatherId\" left join \"T_Sheep\" sm ON sm.\"Id\" = s.\"MotherId\" join \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" join \"T_Sheepfold\" f ON f.\"Id\" = s.\"SheepfoldId\" and  f.\"SysFlag\"=FALSE join \"T_User\" u ON u.\"Id\" = s.\"OperatorId\" join \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\"";

            string countSql = "SELECT COUNT (s.\"Id\") FROM \"T_Sheep\" s left JOIN \"T_Sheep\" sf ON sf.\"Id\" = s.\"FatherId\" left JOIN \"T_Sheep\" sm ON sm.\"Id\" = s.\"MotherId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Sheepfold\" f ON f.\"Id\" = s.\"SheepfoldId\" and  f.\"SysFlag\"=FALSE JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\"";

            return GetPagedData<Sheep, SheepFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<Sheep> GetAllSheep(SheepFilter filter, int pageIndex, int pageSize, out int totalCounts)
        {
            string querySql = "SELECT \"row_number\"() OVER(ORDER BY s.\"CreateTime\" desc) \"rownum\",s.\"Id\", s.\"SerialNumber\",s.\"Gender\", s.\"GrowthStage\",s.\"BirthWeight\", s.\"CompatriotNumber\", s.\"Birthday\", s.\"AblactationWeight\", s.\"AblactationDate\", s.\"Origin\",s.\"Status\",s.\"CreateTime\", s.\"Remark\", sf.\"SerialNumber\" AS \"FatherSerialNumber\", sm.\"SerialNumber\" AS \"MotherSerialNumber\", b.\"Name\" AS \"BreedName\", f.\"Name\" AS \"SheepfoldName\", u.\"UserName\" AS \"OperatorName\", e.\"Name\" AS \"PrincipalName\" FROM \"T_Sheep\" s left join \"T_Sheep\" sf ON sf.\"Id\" = s.\"FatherId\" left join \"T_Sheep\" sm ON sm.\"Id\" = s.\"MotherId\" join \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" join \"T_Sheepfold\" f ON f.\"Id\" = s.\"SheepfoldId\" join \"T_User\" u ON u.\"Id\" = s.\"OperatorId\" join \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\"";

            string countSql = "SELECT COUNT (s.\"Id\") FROM \"T_Sheep\" s left JOIN \"T_Sheep\" sf ON sf.\"Id\" = s.\"FatherId\" left JOIN \"T_Sheep\" sm ON sm.\"Id\" = s.\"MotherId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Sheepfold\" f ON f.\"Id\" = s.\"SheepfoldId\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\"";

            return GetPagedData<Sheep, SheepFilter>(pageIndex, pageSize, out totalCounts, countSql, querySql, filter);
        }

        public Sheep GetSheepById(string id)
        {
            string sql = "SELECT s.\"Id\", s.\"SerialNumber\", s.\"BreedId\", s.\"Gender\", s.\"GrowthStage\", s.\"SheepfoldId\", s.\"BirthWeight\", s.\"CompatriotNumber\", s.\"Birthday\", s.\"AblactationWeight\", s.\"AblactationDate\", s.\"Origin\", s.\"FatherId\", s.\"MotherId\", s.\"Status\", s.\"PrincipalId\", s.\"OperatorId\", s.\"CreateTime\", s.\"Remark\" FROM \"T_Sheep\" s where s.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return GetData<Sheep>(sql, pms).FirstOrDefault();
        }

        public List<Sheep4FeedOutWarehouse> GetSheep4FeedOutWarehouse(List<string> shepfolds)
        {
            string sql = string.Format("select s.\"Id\",s.\"SheepfoldId\",s.\"GrowthStage\"  from \"T_Sheep\" s where \"SheepfoldId\" in ('{0}')", string.Join("','", shepfolds));
            return GetData<Sheep4FeedOutWarehouse>(sql, null);
        }

        public bool IsSheepExist(string id)
        {
            string sql = "SELECT count(s.\"Id\") FROM \"T_Sheep\" s where s.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            object obj = AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
            return obj == null ? false : Convert.ToInt32(obj.ToString()) == 1;
        }

        public List<Sheep> GetTwoSheeps(string maleId, string femaleId)
        {
            string sql = "SELECT \"Id\",\"Gender\" FROM \"T_Sheep\" s where \"GrowthStage\"=" + (int)GrowthStageEnum.StudSheep + " and (\"Status\"=" + (int)SheepStatusEnum.Nomal + "or \"Status\"=" + (int)SheepStatusEnum.Outer + ") and (s.\"Id\"=:maleId or s.\"Id\"=:femaleId)";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("maleId", maleId);
            pms.AddWithValue("femaleId", femaleId);
            return GetData<Sheep>(sql, pms);
        }

        public List<Sheep> GetSheepByIds(IEnumerable<string> ids)
        {
            string sql = "select \"Id\",\"SheepfoldId\" from \"T_Sheep\" where \"Id\" in('" + string.Join("','", ids) + "')";
            return GetData<Sheep>(sql, null);
        }

        public int GetBreedCountByName(string name)
        {
            string sql = "select count(\"Id\") from \"T_Breed\" where \"Name\"=:name";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            return GetDataCount(sql, pms);
        }

        public void AddBreed(string id, string name, string description, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Breed\" (\"Id\",\"Name\",\"Description\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:name,:description,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("description", description);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public List<Sheepfold> GetSheepfold(SheepfoldFilter filter)
        {
            string sql = "SELECT s.\"Id\", s.\"Name\", u.\"UserName\" AS \"OperatorName\", administrator.\"Name\" AS \"AdministratorName\", \"count\" (sp.\"Id\") AS \"SheepCount\", s.\"CreateTime\", s.\"Remark\" FROM \"T_Sheepfold\" s JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\" JOIN \"T_Employee\" administrator ON administrator.\"Id\" = s.\"Administrator\" left JOIN \"T_Sheep\" sp ON sp.\"SheepfoldId\" = s.\"Id\" {0} GROUP BY s.\"Administrator\", s.\"CreateTime\", s.\"Id\", s.\"Name\", s.\"Remark\", u.\"UserName\", administrator.\"Name\"";
            return GetDataWithFormate<Sheepfold, SheepfoldFilter>(sql, filter);
        }
        public List<Sheepfold> GetSheepfold(SheepfoldFilter filter, int rowsCount)
        {
            string sql = "SELECT \"row_number\" () OVER (ORDER BY s.\"Name\") \"rownum\", s.\"Id\", s.\"Name\", u.\"UserName\" AS \"OperatorName\", administrator.\"Name\" AS \"AdministratorName\", \"count\" (sp.\"Id\") AS \"SheepCount\", s.\"CreateTime\", s.\"Remark\" FROM \"T_Sheepfold\" s JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\" JOIN \"T_Employee\" administrator ON administrator.\"Id\" = s.\"Administrator\" left JOIN \"T_Sheep\" sp ON sp.\"SheepfoldId\" = s.\"Id\" {0} GROUP BY s.\"Administrator\", s.\"CreateTime\", s.\"Id\", s.\"Name\", s.\"Remark\", u.\"UserName\", administrator.\"Name\"";
            return GetRuledRowsDataWithFormate<Sheepfold, SheepfoldFilter>(rowsCount, sql, filter);
        }
        public string GetSysSheepfoldId(string administrator, string operatorId)
        {
            string sql = "select sf.\"Id\" from \"T_Sheepfold\" sf where sf.\"SysFlag\"=true";
            object obj = AdoTemplate.ExecuteScalar(CommandType.Text, sql);
            string id;
            if (obj == null)
            {
                id = Guid.NewGuid().ToString();
                sql = "INSERT into \"T_Sheepfold\" (\"Id\",\"Name\",\"Administrator\",\"OperatorId\",\"CreateTime\",\"SysFlag\",\"Remark\")VALUES(:id,:name,(select u.\"Id\" from \"T_User\" u where u.\"UserName\"='admin'),:operatorId,:createTime,:sysFlag,:remark)";

                IDbParameters pms = AdoTemplate.CreateDbParameters();
                pms.AddWithValue("id", id);
                pms.AddWithValue("name", "系统预留");
                pms.AddWithValue("operatorId", operatorId);
                pms.AddWithValue("createTime", DateTime.Now);
                pms.AddWithValue("sysFlag", true);
                pms.AddWithValue("remark", "系统预留，虚拟圈舍，死亡或者出售");

                AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
            }
            else
                id = obj.ToString();
            return id;
        }

        public Breed GetBreedByName(string name)
        {
            string sql = "select \"Id\",\"Name\",\"Description\",\"Remark\" from \"T_Breed\" WHERE \"Name\"=:name";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            return GetData<Breed>(sql, pms).FirstOrDefault();
        }

        public void AddSheep(string id, string serialNumber, string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, string fatherId, string motherId, string sheepfoldId, SheepStatusEnum sheepStatusEnum, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Sheep\" (\"Id\",\"SerialNumber\",\"BreedId\",\"Gender\",\"GrowthStage\",\"SheepfoldId\",\"BirthWeight\",\"CompatriotNumber\",\"Birthday\",\"Origin\",\"FatherId\",\"MotherId\",\"Status\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:serialNumber,:breedId,:gender,:growthStage,:sheepfoldId,:birthWeight,:compatriotNumber,:birthday,:origin,:fatherId,:motherId,:status,:principalId,:operaterId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("serialNumber", serialNumber);
            pms.AddWithValue("breedId", breedId);
            pms.AddWithValue("gender", (int)gender);
            pms.AddWithValue("growthStage", (int)growthStage);
            pms.AddWithValue("sheepfoldId", sheepfoldId);
            pms.AddWithValue("birthWeight", birthWeight);
            pms.AddWithValue("compatriotNumber", compatriotNumber);
            pms.AddWithValue("birthday", birthDay);
            pms.AddWithValue("origin", (int)origin);
            pms.AddWithValue("fatherId", fatherId);
            pms.AddWithValue("motherId", motherId);
            pms.AddWithValue("status", (int)sheepStatusEnum);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operaterId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        [Transaction]
        public void AddSheep(string id, string serialNumber, string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, string fatherId, string motherId, string sheepfoldId, SheepStatusEnum sheepStatusEnum, string principalId, string operatorId, DateTime createTime, string remark, string buySource, decimal buyMoney, float? buyWeight, DateTime buyOperationDate, string buyPrincipalId, string buyRemark)
        {
            string sql = "INSERT into \"T_Sheep\" (\"Id\",\"SerialNumber\",\"BreedId\",\"Gender\",\"GrowthStage\",\"SheepfoldId\",\"BirthWeight\",\"CompatriotNumber\",\"Birthday\",\"Origin\",\"FatherId\",\"MotherId\",\"Status\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:serialNumber,:breedId,:gender,:growthStage,:sheepfoldId,:birthWeight,:compatriotNumber,:birthday,:origin,:fatherId,:motherId,:status,:principalId,:operaterId,:createTime,:remark);INSERT into \"T_BuySheep\" (\"ExpenditureId\",\"SheepId\",\"Source\",\"Weight\")VALUES(:expenditureId,:sheepId,:source,:buyWeight);INSERT into \"T_Expenditure\" (\"Id\",\"Money\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:exId,:money,:buyOperationDate,:buyPrincipalId,:buyOperatorId,:buyCreateTime,:buyRemark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("serialNumber", serialNumber);
            pms.AddWithValue("breedId", breedId);
            pms.AddWithValue("gender", (int)gender);
            pms.AddWithValue("growthStage", (int)growthStage);
            pms.AddWithValue("sheepfoldId", sheepfoldId);
            pms.AddWithValue("birthWeight", birthWeight);
            pms.AddWithValue("compatriotNumber", compatriotNumber);
            pms.AddWithValue("birthday", birthDay);
            pms.AddWithValue("origin", (int)origin);
            pms.AddWithValue("fatherId", fatherId);
            pms.AddWithValue("motherId", motherId);
            pms.AddWithValue("status", (int)sheepStatusEnum);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operaterId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            string expenditureId = Guid.NewGuid().ToString();

            pms.AddWithValue("expenditureId", expenditureId);
            pms.AddWithValue("sheepId", id);
            pms.AddWithValue("source", buySource);
            pms.AddWithValue("buyWeight", buyWeight);

            pms.AddWithValue("exId", expenditureId);
            pms.AddWithValue("money", buyMoney);
            pms.AddWithValue("buyOperationDate", buyOperationDate);
            pms.AddWithValue("buyPrincipalId", buyPrincipalId);
            pms.AddWithValue("buyOperatorId", operatorId);
            pms.AddWithValue("buyCreateTime", createTime);
            pms.AddWithValue("buyRemark", buyRemark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public int GetCurMonthBirthCount()
        {
            string sql = "select count(\"Id\") from \"T_Sheep\" where EXTRACT(YEAR from \"CreateTime\")=EXTRACT(YEAR from now()) and EXTRACT(MONTH from \"CreateTime\")=EXTRACT(MONTH from now())";
            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql));
        }

        public void UpdateSheep(string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, float? ablactationWeight, DateTime? ablactationDate, string fatherId, string motherId, string sheepfoldId, SheepStatusEnum sheepStatusEnum, string principalId, string remark, string id)
        {
            string sql = "update \"T_Sheep\"  set \"BreedId\"=:breedId,\"Gender\"=:gender,\"GrowthStage\"=:growthStage,\"SheepfoldId\"=:sheepfoldId,\"BirthWeight\"=:birthWeight,\"CompatriotNumber\"=:compatriotNumber,\"Birthday\"=:birthday,\"Origin\"=:origin, \"AblactationWeight\"=:ablactationWeight,\"AblactationDate\"=:ablactationDate,\"FatherId\"=:fatherId,\"MotherId\"=:motherId,\"Status\"=:status,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();

            pms.AddWithValue("breedId", breedId);
            pms.AddWithValue("gender", (int)gender);
            pms.AddWithValue("growthStage", (int)growthStage);
            pms.AddWithValue("sheepfoldId", sheepfoldId);
            pms.AddWithValue("birthWeight", birthWeight);
            pms.AddWithValue("compatriotNumber", compatriotNumber);
            pms.AddWithValue("birthday", birthDay);
            pms.AddWithValue("origin", (int)origin);
            pms.AddWithValue("ablactationWeight", ablactationWeight);
            pms.AddWithValue("ablactationDate", ablactationDate);
            pms.AddWithValue("fatherId", fatherId);
            pms.AddWithValue("motherId", motherId);
            pms.AddWithValue("status", (int)sheepStatusEnum);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public int GetSheepCountBySerialNum(string serialNumber)
        {
            string sql = "select count(1) from \"T_Sheep\" s where s.\"SerialNumber\"=:serialNumber";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("serialNumber", serialNumber);

            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms));
        }

        public int GetSheepFoldCountByName(string name)
        {
            string sql = "select count(\"Id\") from \"T_Sheepfold\" where \"Name\"=:name";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            return GetDataCount(sql, pms);
        }

        public void AddSheepFold(string id, string name, string administrator, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Sheepfold\" (\"Id\",\"Name\",\"Administrator\",\"OperatorId\",\"CreateTime\",\"SysFlag\",\"Remark\")VALUES(:id,:name,:administrator,:operatorId,:createTime,false,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("administrator", administrator);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }
        public Sheepfold GetSheepFoldById(string id)
        {
            string sql = "select s.\"Id\",s.\"Name\",s.\"Administrator\",s.\"OperatorId\",s.\"CreateTime\",s.\"Remark\" from \"T_Sheepfold\" as s where s.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return GetData<Sheepfold>(sql, pms).FirstOrDefault();
        }
        public void UpdateSheepFold(string name, string administrator, string remark, string id)
        {
            string sql = "UPDATE \"T_Sheepfold\" SET \"Name\"=:name,\"Administrator\"=:administrator,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            pms.AddWithValue("administrator", administrator);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public Farm GetFarm()
        {
            string sql = "select \"Id\",\"Name\",\"Contacts\",\"Phone\",\"Address\",\"Code\",\"BusinessScope\",\"Qualifications\",\"Remark\" from \"T_Farm\"";
            return GetData<Farm>(sql, null).FirstOrDefault();
        }

        public void AddFarm(string id, string name, string contacts, string phone, string address, string code, string businessScope, string qualifications, string remark)
        {
            string sql = "insert into \"T_Farm\"(\"Id\",\"Name\",\"Contacts\",\"Phone\",\"Address\",\"Code\",\"BusinessScope\",\"Qualifications\",\"Remark\") values (:id,:name,:contacts,:phone,:address,:code,:businessScope,:qualifications,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("contacts", contacts);
            pms.AddWithValue("phone", phone);
            pms.AddWithValue("address", address);
            pms.AddWithValue("code", code);
            pms.AddWithValue("businessScope", businessScope);
            pms.AddWithValue("qualifications", qualifications);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void UpdateFarm(string name, string contacts, string phone, string address, string code, string businessScope, string qualifications, string remark, string id)
        {
            string sql = "update \"T_Farm\" set \"Name\"=:name,\"Contacts\"=:contacts,\"Phone\"=:phone,\"Address\"=:address,\"Code\"=:code,\"BusinessScope\"=:businessScope, \"Qualifications\"=:qualifications,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            pms.AddWithValue("contacts", contacts);
            pms.AddWithValue("phone", phone);
            pms.AddWithValue("address", address);
            pms.AddWithValue("code", code);
            pms.AddWithValue("businessScope", businessScope);
            pms.AddWithValue("qualifications", qualifications);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        #endregion
    }
}
