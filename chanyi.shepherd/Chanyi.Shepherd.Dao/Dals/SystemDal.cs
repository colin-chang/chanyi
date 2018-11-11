using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Spring.Data.Common;
using Spring.Transaction.Interceptor;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.Filter.System;
using Chanyi.Shepherd.QueryModel.Model.System;
using Chanyi.Shepherd.QueryModel.UpdateModel.System;
using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region System

        public List<Login> GetLoginMsg(string userName)
        {
            string sql = "select u.\"IsEnabled\",u.\"LastErrorTime\",u.\"ErrorTimes\" from \"T_User\" u where u.\"UserName\"=:userName";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("userName", userName);
            return GetData<Login>(sql, pms);
        }

        public string Login(string userName, string password)
        {
            string sql = "select \"Id\" from \"T_User\" u where u.\"UserName\"=:userName and u.\"Password\"=:password and u.\"IsEnabled\"=true";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("userName", userName);
            pms.AddWithValue("password", GetPwd(password));
            object result = AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);

            return result == null ? string.Empty : result.ToString();
        }

        public void UpdateLoginErrorTimes(string userName, int errorTimes, DateTime lastErrorTime)
        {
            string sql = "update \"T_User\"  set \"ErrorTimes\"=:errorTimes,\"LastErrorTime\"=:lastErrorTime where \"UserName\"=:userName";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("errorTimes", errorTimes);
            pms.AddWithValue("lastErrorTime", lastErrorTime);
            pms.AddWithValue("userName", userName);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void UpdatePassword(string password, string id)
        {
            string sql = "UPDATE \"T_User\" SET \"Password\"=:password where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("password", GetPwd(password));
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public int GetUserCount(string password, string id)
        {
            string sql = "select count(\"Id\") from \"T_User\" where \"Id\"=:id and \"Password\"=:password";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("password", GetPwd(password));
            pms.AddWithValue("id", id);

            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms));
        }

        public List<SheepParameter> GetSheepParameters()
        {
            string sql = "select s.\"Id\",s.\"IsRemaindful\",s.\"Value\",s.\"Type\",p.\"Range\",s.\"Remark\" from \"T_SheepParameter\" p join \"T_Settings\" s on s.\"Id\"=p.\"SettingsId\"";

            return GetData<SheepParameter>(sql, null);
        }

        [Transaction]
        public void UpdateSheepParameters(List<UpdateSheepParameter> list, int deliveryRangeDays)
        {
            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();

            for (int i = 0; i < list.Count; i++)
            {
                sb.AppendFormat("UPDATE \"T_SheepParameter\" SET \"Range\"=:range{0} where \"SettingsId\"=:sheepParameterSettingsId{0};UPDATE \"T_Settings\" SET \"Value\"=:value{0},\"IsRemaindful\"=:isRemaindful{0},\"Remark\"=:remark{0} where \"Id\"=:id{0};INSERT into \"T_SettingsLog\" (\"Id\",\"SettingsId\",\"Category\",\"NewValue\",\"OperatorId\",\"CreateTime\")VALUES(:logId{0},:settingsId{0},:category{0},:newValue{0},:operatorId{0},:createTime{0});", i);

                pms.AddWithValue("range" + i, deliveryRangeDays);
                pms.AddWithValue("sheepParameterSettingsId" + i, list[i].Id);

                pms.AddWithValue("value" + i, list[i].Value);
                pms.AddWithValue("remark" + i, list[i].Remark);
                pms.AddWithValue("isRemaindful" + i, list[i].IsRemaindful);
                pms.AddWithValue("id" + i, list[i].Id);

                pms.AddWithValue("logId" + i, Guid.NewGuid());
                pms.AddWithValue("settingsId" + i, list[i].Id);
                pms.AddWithValue("category" + i, (int)list[i].Type);
                pms.AddWithValue("newValue" + i, list[i].Value);
                pms.AddWithValue("operatorId" + i, list[i].OperatorId);
                pms.AddWithValue("createTime" + i, DateTime.Now);

            }
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }

        public List<FeedCritical> GetFeedCritical()
        {
            string sql = "SELECT s.\"Id\",fin.\"KindId\", n.\"Name\", A .\"Name\" AS \"Area\", d.\"Value\" AS \"FeedType\", CASE WHEN s.\"Value\" IS NULL THEN '0' ELSE s.\"Value\" END, CASE WHEN s.\"IsRemaindful\" IS NULL THEN FALSE ELSE \"IsRemaindful\" END FROM \"T_Critical\" C JOIN \"T_Settings\" s ON s.\"Id\" = C .\"SettingsId\" RIGHT JOIN \"T_FeedInventory\" fin ON fin.\"KindId\" = C .\"KindId\" JOIN \"T_Feed\" f ON f.\"Id\" = fin.\"KindId\" JOIN \"T_InputName\" n ON n.\"Id\" = f.\"NameId\" left JOIN \"T_Area\" A ON A .\"Id\" = f.\"AreaId\" JOIN \"T_InventoryDict\" d ON d.\"Id\" = f.\"TypeId\" where d.\"Category\"=:category ORDER BY s.\"Id\", \"Name\" asc";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("category", feedTypeCategory);
            return GetData<FeedCritical>(sql, pms);
        }
        public List<MedicineCritical> GetMedicineCritical()
        {
            string sql = "SELECT s.\"Id\",mi.\"KindId\", n.\"Name\", cr.\"Department\" AS \"Manufacturer\",d.\"Value\" as \"MedicineType\", CASE WHEN s.\"Value\" IS NULL THEN '0' ELSE s.\"Value\" END, CASE WHEN s.\"IsRemaindful\" IS NULL THEN FALSE ELSE s.\"IsRemaindful\" END FROM \"T_Critical\" C JOIN \"T_Settings\" s ON s.\"Id\" = C .\"SettingsId\" RIGHT JOIN \"T_MedicineInventory\" mi ON mi.\"KindId\" = C .\"KindId\" join \"T_MedicineCrucial\" mc on mc.\"Id\"= mi.\"KindId\" JOIN \"T_Medicine\" M ON M .\"Id\" = mc.\"KindId\" JOIN \"T_InputName\" n ON n.\"Id\" = M .\"NameId\" JOIN \"T_Cooperater\" cr ON cr.\"Id\" = M .\"ManufacturerId\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=M.\"TypeId\"  where d.\"Category\"=:category  ORDER BY s.\"Id\", n.\"Name\" asc";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("category", medicineTypeCategory);
            return GetData<MedicineCritical>(sql, pms);
        }

        [Transaction]
        public void AddCritical(List<UpdateCritical> listAdd)
        {
            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();

            for (int i = 0; i < listAdd.Count; i++)
            {
                sb.AppendFormat("INSERT into \"T_Critical\" (\"SettingsId\",\"KindId\")VALUES(:criticalId{0},:kindId{0});INSERT into \"T_Settings\" (\"Id\",\"Value\",\"IsRemaindful\",\"Type\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id{0},:value{0},:isRemaindful{0},:type{0},:operatorId{0},:createTime{0},:remark{0});INSERT into \"T_SettingsLog\" (\"Id\",\"SettingsId\",\"Category\",\"NewValue\",\"OperatorId\",\"CreateTime\")VALUES(:logId{0},:settingsId{0},:category{0},:newValue{0},:logOperatorId{0},:logCreateTime{0});", i);

                string settingsId = Guid.NewGuid().ToString();

                pms.AddWithValue("criticalId" + i, settingsId);
                pms.AddWithValue("kindId" + i, listAdd[i].KindId);

                pms.AddWithValue("id" + i, settingsId);
                pms.AddWithValue("value" + i, listAdd[i].Value);
                pms.AddWithValue("isRemaindful" + i, listAdd[i].IsRemaindful);
                pms.AddWithValue("type" + i, (int)listAdd[i].Type);
                pms.AddWithValue("operatorId" + i, listAdd[i].OperatorId);
                pms.AddWithValue("createTime" + i, DateTime.Now);
                pms.AddWithValue("remark" + i, listAdd[i].Remark);

                pms.AddWithValue("logId" + i, Guid.NewGuid());
                pms.AddWithValue("settingsId" + i, settingsId);
                pms.AddWithValue("category" + i, (int)listAdd[i].Type);
                pms.AddWithValue("newValue" + i, listAdd[i].Value);
                pms.AddWithValue("logOperatorId" + i, listAdd[i].OperatorId);
                pms.AddWithValue("logCreateTime" + i, DateTime.Now);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }
        [Transaction]
        public void UpdateCritical(List<UpdateCritical> listUpdate)
        {
            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            for (int i = 0; i < listUpdate.Count; i++)
            {
                sb.AppendFormat("UPDATE \"T_Settings\" SET \"Value\"=:value{0},\"IsRemaindful\"=:isRemaindful{0},\"Remark\"=:remark{0} where \"Id\"=:id{0};INSERT into \"T_SettingsLog\" (\"Id\",\"SettingsId\",\"Category\",\"NewValue\",\"OperatorId\",\"CreateTime\")VALUES(:logId{0},:settingsId{0},:category{0},:newValue{0},:logOperatorId{0},:logCreateTime{0});", i);

                pms.AddWithValue("value" + i, listUpdate[i].Value);
                pms.AddWithValue("isRemaindful" + i, listUpdate[i].IsRemaindful);
                pms.AddWithValue("remark" + i, listUpdate[i].Remark);
                pms.AddWithValue("id" + i, listUpdate[i].Id);

                pms.AddWithValue("logId" + i, Guid.NewGuid());
                pms.AddWithValue("settingsId" + i, listUpdate[i].Id);
                pms.AddWithValue("category" + i, (int)listUpdate[i].Type);
                pms.AddWithValue("newValue" + i, listUpdate[i].Value);
                pms.AddWithValue("logOperatorId" + i, listUpdate[i].OperatorId);
                pms.AddWithValue("logCreateTime" + i, DateTime.Now);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }

        public List<PreDeliveryRemaindful> GetPreDeliveryRemaindful(SheepParameter pm)
        {
            string sql = "SELECT s.\"SerialNumber\", s.\"Origin\", b.\"Name\" AS \"Breed\", f.\"Name\" AS \"Sheepfold\" FROM \"T_Mating\" M JOIN \"T_Sheep\" s ON s.\"Id\" = M .\"FemaleId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Sheepfold\" f ON f.\"Id\" = s.\"SheepfoldId\" WHERE \"IsRemindful\" = TRUE AND \"MatingDate\" <:minValue  and s.\"Status\"=:status ORDER BY \"MatingDate\" DESC";
            //不生不堕胎就一直提醒,在AddMating时对前边所有的该母羊配种记录进行不提醒操作
            // AND \"MatingDate\" <:maxValue
            //DateTime maxValue = DateTime.Now.AddDays(0 - (Convert.ToInt32(pm.Value) - pm.Range + 1));
            DateTime minValue = DateTime.Now.AddDays(0 - (Convert.ToInt32(pm.Value) - pm.Range + 1));
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("minValue", minValue);
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);
            //pms.AddWithValue("maxValue", maxValue);

            return GetData<PreDeliveryRemaindful>(sql, pms);
        }

        public List<PreAblactationRemaindful> GetPreAblactationRemaindful(int value)
        {
            string sql = "select s.\"SerialNumber\",s.\"Origin\",b.\"Name\" as \"Breed\",f.\"Name\" as \"Sheepfold\" from \"T_Sheep\" s join \"T_Breed\" b on b.\"Id\"=s.\"BreedId\" JOIN \"T_Sheepfold\" f on f.\"Id\"=s.\"SheepfoldId\" where \"AblactationDate\" is NULL and \"GrowthStage\"=:growthStage and \"Status\"=:status and \"Birthday\" <=:value";
            IDbParameters pms = AdoTemplate.CreateDbParameters();

            pms.AddWithValue("growthStage", (int)GrowthStageEnum.Lamb);
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);
            pms.AddWithValue("value", DateTime.Now.AddDays(0 - value));

            return GetData<PreAblactationRemaindful>(sql, pms);
        }
        public List<SheepParameter> GetSheepParameterValue(SettingsEnum settingsEnum)
        {
            string sql = "SELECT \"Value\",\"Range\",\"IsRemaindful\" from \"T_SheepParameter\" p join \"T_Settings\" s on s.\"Id\"=p.\"SettingsId\" where \"Type\"=:type";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("type", (int)settingsEnum);

            return GetData<SheepParameter>(sql, pms);
        }

        public List<FeedRemaindful> GetFeedRemaindful()
        {
            string sql = "select n.\"Name\",a.\"Name\" as \"Area\",d.\"Value\" as \"Type\",fin.\"Amount\" from \"T_FeedInventory\" fin  join \"T_Critical\" c on c.\"KindId\"=fin.\"KindId\" join \"T_Settings\" s on s.\"Id\"=c.\"SettingsId\" join \"T_Feed\" f on f.\"Id\"=fin.\"KindId\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" where fin.\"Amount\"<=cast(s.\"Value\" as int)  and s.\"IsRemaindful\"=true and d.\"Category\"=:category";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("category", feedTypeCategory);
            return GetData<FeedRemaindful>(sql, pms);
        }

        public List<MedicineRemaindful> GetMedicineRemaindful()
        {
            string sql = "SELECT n.\"Name\",cr.\"Department\" as \"Manufacturer\",mi.\"Amount\",d.\"Value\" as \"Type\" from \"T_MedicineInventory\" mi join \"T_Critical\" c on c.\"KindId\"=mi.\"KindId\" join \"T_Settings\" s on s.\"Id\"=c.\"SettingsId\" join \"T_MedicineCrucial\" mc on mc.\"Id\"= mi.\"KindId\" join \"T_Medicine\" m on m.\"Id\"=mc.\"KindId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Cooperater\" cr on cr.\"Id\"=m.\"ManufacturerId\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" where mi.\"Amount\"<cast(s.\"Value\" as int) and s.\"IsRemaindful\"=true  and d.\"Category\"=:category";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("category", medicineTypeCategory);
            return GetData<MedicineRemaindful>(sql, pms);
        }

        public List<PreDeliveryRemaindful> GetPreDeliverySerialNumber(SheepParameter pm)
        {
            string sql = "SELECT s.\"SerialNumber\" FROM \"T_Mating\" M JOIN \"T_Sheep\" s ON s.\"Id\" = M .\"FemaleId\" WHERE \"IsRemindful\" = TRUE AND \"MatingDate\" <:minValue and s.\"Status\"=:status  ORDER BY \"MatingDate\" DESC";

            DateTime minValue = DateTime.Now.AddDays(0 - (Convert.ToInt32(pm.Value) - pm.Range + 1));
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("minValue", minValue);
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);

            return GetData<PreDeliveryRemaindful>(sql, pms);
        }

        public List<PreAblactationRemaindful> GetPreAblactationSerialNumber(int value)
        {
            string sql = "select s.\"SerialNumber\" from \"T_Sheep\" s where \"AblactationDate\" is NULL and \"GrowthStage\"=:growthStage and \"Status\"=:status and \"Birthday\" <=:value";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("growthStage", (int)GrowthStageEnum.Lamb);
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);
            pms.AddWithValue("value", DateTime.Now.AddDays(0 - value));

            return GetData<PreAblactationRemaindful>(sql, pms);
        }

        public void AddUser(string id, string userName, string password, int errorTimes, DateTime lastErrorTime, string operatorId, DateTime createTime, bool isEnabled, string remark)
        {
            string sql = "insert into \"T_User\" (\"Id\",\"UserName\",\"Password\",\"ErrorTimes\",\"LastErrorTime\",\"OperatorId\",\"CreateTime\",\"IsEnabled\",\"Remark\")VALUES(:Id,:UserName,:Password,:ErrorTimes,:LastErrorTime,:OperatorId,:CreateTime,:IsEnabled,:Remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("userName", userName);
            pms.AddWithValue("password", GetPwd(password));
            pms.AddWithValue("errorTimes", errorTimes);
            pms.AddWithValue("lastErrorTime", lastErrorTime);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("isEnabled", isEnabled);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public int GetUserCount(string userName)
        {
            string sql = "select count(\"Id\") from \"T_User\" where \"UserName\"=:userName";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("userName", userName);
            object obj = AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
            return obj == null ? 0 : int.Parse(obj.ToString());
        }


        public void AddRole(string id, string name, string description, string operatorId, DateTime createTime, string remark)
        {
            string sql = "insert into \"T_Role\" (\"Id\",\"Name\",\"Description\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:Id,:Name,:Description,:OperatorId,:CreateTime,:Remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("description", description);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public void AddPermission(string id, string name, string description, string URL, string operatorId, DateTime createTime, string remark)
        {
            string sql = "insert into \"T_Permission\" (\"Id\",\"Name\",\"Descreption\",\"URL\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:Id,:Name,:Description,:URL,:OperatorId,:CreateTime,:Remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("description", description);
            pms.AddWithValue("URL", URL);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public Role GetRoleById(string id)
        {
            string sql = " select r.\"Id\",r.\"Name\",r.\"Description\",r.\"OperatorId\",r.\"CreateTime\",r.\"Remark\" FROM \"T_Role\" as r  where r.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return GetData<Role>(sql, pms).FirstOrDefault();
        }

        public Permission GetPermissionById(string id)
        {
            string sql = " select p.\"Id\",p.\"Name\",p.\"Description\",p.\"URL\",p.\"OperatorId\",p.\"CreateTime\",p.\"Remark\" FROM \"T_Permission\" as p  where p.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return GetData<Permission>(sql, pms).FirstOrDefault();
        }

        public void UpdateRole(string name, string description, string remark, string id)
        {
            string sql = "update  \"T_Role\" set \"Name\"=:name,\"Description\"=:description,\"Remark\"=:remark where \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            pms.AddWithValue("description", description);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public void UpdatePermission(string name, string description, string remark, string id)
        {
            string sql = "update  \"T_Permission\" set \"Name\"=:name,\"Description\"=:description,\"Remark\"=:remark where \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            pms.AddWithValue("description", description);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }


        public List<Permission> GetAllPermissions(PermissionFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\"() OVER(ORDER BY p.\"CreateTime\" desc) \"rownum\", p.\"Id\",p.\"Name\",p.\"Description\",p.\"URL\",u.\"UserName\" as \"OperatorName\",p.\"CreateTime\",p.\"Remark\" from \"T_Permission\" p join \"T_User\" u on u.\"Id\"=p.\"OperatorId\"";

            string countSql = "SELECT COUNT (p.\"Id\") FROM \"T_Permission\" p join \"T_User\" u on u.\"Id\"=p.\"OperatorId\"";

            return GetPagedData<Permission, PermissionFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<Role> GetAllRoles(RoleFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\"() OVER(ORDER BY r.\"CreateTime\" desc) \"rownum\", r.\"Id\",r.\"Name\",r.\"Description\",u.\"UserName\" as \"OperatorName\",r.\"CreateTime\",r.\"Remark\" from \"T_Role\" r join \"T_User\" u on u.\"Id\"=r.\"OperatorId\"";

            string countSql = "SELECT COUNT (r.\"Id\") FROM \"T_Role\" r join \"T_User\" u on u.\"Id\"=r.\"OperatorId\"";

            return GetPagedData<Role, RoleFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<Permission> GetPermissionsByRoleId(string roleId)
        {
            string sql = "select p.\"Id\",p.\"Name\",p.\"Description\",p.\"URL\",u.\"UserName\" as \"OperatorName\",p.\"CreateTime\",p.\"Remark\" from \"T_Permission\" p  join \"T_PermissionRole\" pr on pr.\"PermissionId\"=p.\"Id\" join \"T_User\" u on u.\"Id\"=p.\"OperatorId\" where pr.\"RoleId\"=:roleId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("roleId", roleId);
            return GetData<Permission>(sql, pms);
        }

        public List<Permission> GetAllPermissionByUserId(string userId)
        {
            string sql = "SELECT P .\"Id\", P .\"Name\", P .\"Description\", P .\"URL\", P .\"Remark\" FROM \"T_Permission\" P JOIN \"T_PermissionRole\" pr ON pr.\"PermissionId\" = P .\"Id\" JOIN \"T_Role\" r ON r.\"Id\" = pr.\"RoleId\" JOIN \"T_UserRole\" ur ON ur.\"RoleId\" = r.\"Id\" WHERE ur.\"UserId\" =:userId UNION SELECT P .\"Id\", P .\"Name\", P .\"Description\", P .\"URL\", P .\"Remark\" FROM \"T_Permission\" P JOIN \"T_UserPermission\" up ON up.\"PermissionId\" = P .\"Id\" WHERE up.\"UserId\" =:userId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("userId", userId);
            List<Permission> list = GetData<Permission>(sql, pms);
            //(HashSet<string>)list;//去重
            return list.Where((x, i) => list.FindIndex(z => z.Id == x.Id) == i).ToList();
        }

        public List<Permission> GetPermissionByUserId(string userId)
        {
            string sql = "select p.\"Id\",p.\"Name\",p.\"Description\",p.\"URL\",u.\"UserName\" as \"OperatorName\",p.\"CreateTime\",p.\"Remark\" from \"T_Permission\" p  join \"T_UserPermission\" up on up.\"PermissionId\"=p.\"Id\" join \"T_User\" u on u.\"Id\"=p.\"OperatorId\" where up.\"UserId\"=:userId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("userId", userId);
            return GetData<Permission>(sql, pms);

        }

        public List<Role> GetAllRoleByUserId(string userId)
        {
            string sql = "select r.\"Id\",r.\"Name\",r.\"Description\",u.\"UserName\" as \"OperatorName\",r.\"CreateTime\",r.\"Remark\" FROM \"T_Role\" as r join \"T_User\" u on u.\"Id\"=r.\"OperatorId\" join \"T_UserRole\" ur on ur.\"RoleId\"=r.\"Id\" where ur.\"UserId\"=:userId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("userId", userId);
            return GetData<Role>(sql, pms);
        }

        public List<Permission> GetRoleAvailablePermissions(string keyWord, string roleId)
        {
            string sql = "SELECT p.\"Id\",p.\"Name\",p.\"Description\",p.\"URL\",p.\"Remark\" from \"T_Permission\" p where p.\"Id\" not in ( select pr.\"PermissionId\"  from \"T_PermissionRole\" pr join \"T_Role\" r on pr.\"RoleId\"=r.\"Id\" WHERE r.\"Id\"=:roleId ) and (p.\"Name\" like :keyWord or p.\"Description\" like :keyWord)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("roleId", roleId);
            pms.AddWithValue("keyWord", keyWord.Wrap("%"));
            return GetData<Permission>(sql, pms);
        }

        public List<Permission> GetUserAvailablePermissions(string keyWord, string userId)
        {
            string sql = "SELECT p.\"Id\",p.\"Name\",p.\"Description\",p.\"URL\",p.\"Remark\" from \"T_Permission\" p where p.\"Id\" not in ( select ur.\"PermissionId\"  from \"T_UserPermission\" ur join \"T_User\" u on ur.\"UserId\"=u.\"Id\" WHERE u.\"Id\"=:userId) and (p.\"Name\" like :keyWord or p.\"Description\" like :keyWord)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("userId", userId);
            pms.AddWithValue("keyWord", keyWord.Wrap("%"));
            return GetData<Permission>(sql, pms);
        }

        public List<Role> GetUserAvailableRoles(string keyWord, string userId)
        {
            string sql = "SELECT r.\"Id\", r.\"Name\", r.\"Description\", r.\"Remark\" FROM \"T_Role\" r WHERE r.\"Id\" NOT IN ( SELECT ur.\"RoleId\" FROM \"T_UserRole\" ur JOIN \"T_User\" u ON ur.\"UserId\" = u.\"Id\" WHERE u.\"Id\" =:userId ) AND ( r.\"Name\" LIKE :keyWord OR r.\"Description\" LIKE :keyWord )";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("userId", userId);
            pms.AddWithValue("keyWord", keyWord.Wrap("%"));
            return GetData<Role>(sql, pms);
        }

        public void DeletePermission(string id)
        {
            string sql = "delete FROM \"T_Permission\" where \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void DeleteRole(string id)
        {
            string sql = "delete FROM \"T_Role\" where \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        [Transaction]
        public void GrantPermission2Role(List<string> permissionIds, string roleId)
        {
            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();

            for (int i = 0; i < permissionIds.Count(); i++)
            {
                sb.AppendFormat("insert into \"T_PermissionRole\" (\"Id\",\"PermissionId\",\"RoleId\")VALUES(:Id{0},:PermissionId{0},:RoleId{0});", i);

                pms.AddWithValue("id" + i, Guid.NewGuid().ToString());
                pms.AddWithValue("permissionId" + i, permissionIds[i]);
                pms.AddWithValue("roleId" + i, roleId);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }

        [Transaction]
        public void GrantRole2User(List<string> roleIds, string userId)
        {
            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();

            for (int i = 0; i < roleIds.Count(); i++)
            {
                sb.AppendFormat("insert into \"T_UserRole\" (\"Id\",\"UserId\",\"RoleId\")VALUES(:Id{0},:UserId{0},:RoleId{0});", i);

                pms.AddWithValue("id" + i, Guid.NewGuid().ToString());
                pms.AddWithValue("userId" + i, userId);
                pms.AddWithValue("roleId" + i, roleIds[i]);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }

        [Transaction]
        public void GrantPermission2User(List<string> permissionIds, string userId)
        {
            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();

            for (int i = 0; i < permissionIds.Count(); i++)
            {
                sb.AppendFormat("insert into \"T_UserPermission\" (\"Id\",\"UserId\",\"PermissionId\")VALUES(:Id{0},:UserId{0},:PermissionId{0});", i);

                pms.AddWithValue("id" + i, Guid.NewGuid().ToString());
                pms.AddWithValue("permissionId" + i, permissionIds[i]);
                pms.AddWithValue("userId" + i, userId);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }


        public void DeleteRolePermission(string roleId)
        {
            string sql = "delete FROM \"T_PermissionRole\" where \"RoleId\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", roleId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void DeleteUserRole(string userId)
        {
            string sql = "delete from \"T_UserRole\" where \"UserId\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", userId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void DeleteUserPermission(string userId)
        {
            string sql = "delete from \"T_UserPermission\" where \"UserId\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", userId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void AddSplitYard(string id, string name, string manager)
        {
            string sql = "insert into \"T_SplitYard\" (\"Id\",\"Name\",\"Manager\")VALUES(:Id,:Name,:Manager)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("manager", manager);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public SplitYard GetSplitYardById(string id)
        {
            string sql = " select s.\"Id\",s.\"Name\",s.\"Manager\" FROM \"T_SplitYard\" as s ";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return GetData<SplitYard>(sql, pms).FirstOrDefault();
        }

        public List<SplitYard> GetAllSplitYard(SplitYardFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\"() OVER(ORDER BY s.\"CreateTime\" desc) \"rownum\", s.\"Id\",s.\"Name\",s.\"Manager\" FROM \"T_SplitYard\" as s";

            string countSql = "SELECT COUNT (s.\"Id\") FROM \"T_SplitYard\" as s";

            return GetPagedData<SplitYard, SplitYardFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public void UpdateSplitYard(string name, string manager, string id)
        {
            string sql = "update  \"T_SplitYard\" set \"Name\"=:name,\"Manager\"=:manager where \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            pms.AddWithValue("manager", manager);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        #endregion
    }
}
