using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Spring.Data.Common;
using Spring.Transaction.Interceptor;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.Filter.Group;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;
using Chanyi.Shepherd.QueryModel.Model.Group;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region GroupManage

        [Transaction]
        public void AddMoveSheepfold(List<Sheep> sheeps, string targetSheepfold, string principalId, string operatorId, DateTime createTime, string remark)
        {
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            StringBuilder sbSql = new StringBuilder();


            for (int i = 0; i < sheeps.Count(); i++)
            {
                if (sheeps[i].SheepfoldId == targetSheepfold)
                    continue;

                sbSql.AppendFormat("UPDATE \"T_Sheep\" set \"SheepfoldId\"=:targetSheepfold{0} WHERE \"Id\"=:sId{0};", i);

                sbSql.AppendFormat("INSERT into \"T_MoveSheepfold\" (\"Id\",\"SheepId\",\"SourceSheepfoldId\",\"DestinationSheepfoldId\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id{0},:sheepId{0},:sourceSheepfoldId{0},:destinationSheepfoldId{0},:operationDate{0},:principalId{0},:operatorId{0},:createTime{0},:remark{0});", i);

                pms.AddWithValue("targetSheepfold" + i, targetSheepfold);
                pms.AddWithValue("sId" + i, sheeps[i].Id);

                pms.AddWithValue("id" + i, Guid.NewGuid());
                pms.AddWithValue("sheepId" + i, sheeps[i].Id);
                pms.AddWithValue("sourceSheepfoldId" + i, sheeps[i].SheepfoldId);
                pms.AddWithValue("destinationSheepfoldId" + i, targetSheepfold);
                pms.AddWithValue("operationDate" + i, createTime);
                pms.AddWithValue("principalId" + i, principalId);
                pms.AddWithValue("operatorId" + i, operatorId);
                pms.AddWithValue("createTime" + i, createTime);
                pms.AddWithValue("remark" + i, remark);
            }
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sbSql.ToString(), pms);
        }

        [Transaction]
        public void AddDeathManage(string id, string sheepId, string reason, DeathDisposeEnum dispose, DateTime deathDate, string principalId, string operatorId, DateTime createTime, string remark, string sysSheepfoldId)
        {
            string sql = "INSERT into \"T_DeathManage\" (\"Id\",\"SheepId\",\"Reason\",\"Dispose\",\"DeathDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"IsDel\",\"Remark\")VALUES(:id,:sheepId,:reason,:dispose,:deathDate,:principalId,:operatorId,:createTime,false,:remark);update \"T_Sheep\" set \"Status\"=:status,\"SheepfoldId\"=:sysSheepfoldId where \"Id\"=:sheepId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("sheepId", sheepId);
            pms.AddWithValue("reason", reason);
            pms.AddWithValue("dispose", (int)dispose);
            pms.AddWithValue("deathDate", deathDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("status", (int)SheepStatusEnum.Dead);
            pms.AddWithValue("sysSheepfoldId", sysSheepfoldId);
            //pms.AddWithValue("updateSheepId", sheepId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }
        public List<DeathManage> GetDeathManage(DeathManageFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY d.\"DeathDate\") AS \"rownum\", d.\"Id\", d.\"Reason\", d.\"Dispose\", d.\"DeathDate\", d.\"CreateTime\", s.\"SerialNumber\", s.\"Gender\", b.\"Name\" AS \"BreedName\", s.\"GrowthStage\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", d.\"Remark\" FROM \"T_DeathManage\" d JOIN \"T_Sheep\" s ON s.\"Id\" = d.\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = d.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = d.\"OperatorId\"";

            string countSql = "select count(d.\"Id\") FROM \"T_DeathManage\" d JOIN \"T_Sheep\" s ON s.\"Id\" = d.\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = d.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = d.\"OperatorId\"";

            return GetPagedData<DeathManage, DeathManageFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<DeathManage> GetDeathManage(DeathManageFilter filter, int rowsCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY d.\"DeathDate\") AS \"rownum\", d.\"Id\", d.\"Reason\", d.\"Dispose\", d.\"DeathDate\", d.\"CreateTime\", s.\"SerialNumber\", s.\"Gender\", b.\"Name\" AS \"BreedName\", s.\"GrowthStage\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", d.\"Remark\" FROM \"T_DeathManage\" d JOIN \"T_Sheep\" s ON s.\"Id\" = d.\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = s.\"BreedId\" JOIN \"T_Employee\" e ON e.\"Id\" = d.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = d.\"OperatorId\"";
            return GetRuledRowsData<DeathManage, DeathManageFilter>(rowsCount, querySql, filter);
        }
        public DeathManage GetDeathManageById(string id)
        {
            string sql = "select d.\"Id\",d.\"Reason\",d.\"Dispose\",d.\"DeathDate\",s.\"SerialNumber\",d.\"PrincipalId\",d.\"Remark\",d.\"IsDel\" from \"T_DeathManage\" d join \"T_Sheep\" s on s.\"Id\"=d.\"SheepId\" where d.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return GetData<DeathManage>(sql, pms).FirstOrDefault();
        }
        [Transaction]
        public void DeleteDeathManage(string id)
        {
            string sql = "update \"T_DeathManage\" set \"IsDel\"=true where \"Id\"=:id;update \"T_Sheep\" set \"Status\"=:status where \"Id\"=(SELECT \"SheepId\" from \"T_DeathManage\" where \"Id\"=:dId)";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);
            pms.AddWithValue("dId", id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms).ToString();
        }

        public void UpdateDeathManage(string reason, DeathDisposeEnum dispose, DateTime deathDate, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_DeathManage\" SET \"Reason\"=:reason,\"Dispose\"=:dispose,\"DeathDate\"=:deathDate,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("reason", reason);
            pms.AddWithValue("dispose", (int)dispose);
            pms.AddWithValue("deathDate", deathDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public List<MoveSheepfold> GetMoveSheepfold(MoveSheepfoldFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY m.\"OperationDate\") as \"rownum\",m.\"Id\",s.\"SerialNumber\",ms.\"Name\" as \"SourceSheepfoldName\",md.\"Name\" as \"DestinationSheepfoldName\",m.\"OperationDate\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperationName\",m.\"CreateTime\",m.\"Remark\" from \"T_MoveSheepfold\" m join \"T_Sheep\" s on s.\"Id\"=m.\"SheepId\" join \"T_Sheepfold\" ms on ms.\"Id\"=m.\"SourceSheepfoldId\" join \"T_Sheepfold\" md on md.\"Id\"=m.\"DestinationSheepfoldId\" join \"T_Employee\" e on e.\"Id\"=m.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=m.\"OperatorId\"";

            string countSql = "select count(m.\"Id\") from \"T_MoveSheepfold\" m join \"T_Sheep\" s on s.\"Id\"=m.\"SheepId\" join \"T_Sheepfold\" ms on ms.\"Id\"=m.\"SourceSheepfoldId\" join \"T_Sheepfold\" md on md.\"Id\"=m.\"DestinationSheepfoldId\" join \"T_Employee\" e on e.\"Id\"=m.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=m.\"OperatorId\"";

            return GetPagedData<MoveSheepfold, MoveSheepfoldFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<MoveSheepfold> GetMoveSheepfold(MoveSheepfoldFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY m.\"OperationDate\") as \"rownum\",m.\"Id\",s.\"SerialNumber\",ms.\"Name\" as \"SourceSheepfoldName\",md.\"Name\" as \"DestinationSheepfoldName\",m.\"OperationDate\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperationName\",m.\"CreateTime\",m.\"Remark\" from \"T_MoveSheepfold\" m join \"T_Sheep\" s on s.\"Id\"=m.\"SheepId\" join \"T_Sheepfold\" ms on ms.\"Id\"=m.\"SourceSheepfoldId\" join \"T_Sheepfold\" md on md.\"Id\"=m.\"DestinationSheepfoldId\" join \"T_Employee\" e on e.\"Id\"=m.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=m.\"OperatorId\"";
            return GetRuledRowsData<MoveSheepfold, MoveSheepfoldFilter>(rowsCount, querySql, filter);
        }

        #endregion
    }
}
