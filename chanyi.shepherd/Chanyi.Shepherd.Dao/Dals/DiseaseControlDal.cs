using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Spring.Data.Common;
using Spring.Transaction.Interceptor;

using Chanyi.Shepherd.QueryModel.Filter.DiseaseControl;
using Chanyi.Shepherd.QueryModel.Model.DiseaseControl;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region DiseaseControl

        public void AddAntiepidemic(string id, string name, string vaccine, DateTime executeDate, string effect, string sheepFlock, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Antiepidemic\" (\"Id\",\"Name\",\"Vaccine\",\"ExecuteDate\",\"Effect\",\"SheepFlock\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:name,:vaccine,:executeDate,:effect,:sheepFlock,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("vaccine", vaccine);
            pms.AddWithValue("executeDate", executeDate);
            pms.AddWithValue("effect", effect);
            pms.AddWithValue("sheepFlock", sheepFlock);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public List<Antiepidemic> GetAntiepidemic(AntiepidemicFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY a.\"CreateTime\" desc) as \"rownum\" ,a.\"Id\",a.\"Name\",a.\"Vaccine\",a.\"ExecuteDate\",a.\"Effect\",a.\"SheepFlock\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as\"OperatorName\",a.\"CreateTime\",a.\"Remark\" from \"T_Antiepidemic\" a join \"T_Employee\" e on e.\"Id\"=a.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=a.\"OperatorId\"";

            string countSql = "select count(a.\"Id\") from \"T_Antiepidemic\" a join \"T_Employee\" e on e.\"Id\"=a.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=a.\"OperatorId\"";

            return GetPagedData<Antiepidemic, AntiepidemicFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<Antiepidemic> GetAntiepidemic(AntiepidemicFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY a.\"CreateTime\" desc) as \"rownum\" ,a.\"Id\",a.\"Name\",a.\"Vaccine\",a.\"ExecuteDate\",a.\"Effect\",a.\"SheepFlock\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as\"OperatorName\",a.\"CreateTime\",a.\"Remark\" from \"T_Antiepidemic\" a join \"T_Employee\" e on e.\"Id\"=a.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=a.\"OperatorId\"";
            return GetRuledRowsData<Antiepidemic, AntiepidemicFilter>(rowsCount, querySql, filter);
        }

        public Antiepidemic GetAntiepidemicById(string id)
        {
            string sql = "select a.\"Id\",a.\"Name\",a.\"Vaccine\",a.\"ExecuteDate\",a.\"Effect\",a.\"SheepFlock\",a.\"PrincipalId\",a.\"OperatorId\",a.\"CreateTime\",a.\"Remark\" from \"T_Antiepidemic\" a where a.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return GetData<Antiepidemic>(sql, pms).FirstOrDefault();
        }

        public void UpdateAntiepidemic(string name, string vaccine, DateTime executeDate, string effect, string sheepFlock, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_Antiepidemic\" SET \"Name\"=:name,\"Vaccine\"=:vaccine,\"ExecuteDate\"=:executeDate,\"Effect\"=:effect,\"SheepFlock\"=:sheepFlock,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("vaccine", vaccine);
            pms.AddWithValue("executeDate", executeDate);
            pms.AddWithValue("effect", effect);
            pms.AddWithValue("sheepFlock", sheepFlock);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public void AddAntiepidemicPlan(string id, string name, string vaccine, DateTime planExecuteDate, string sheepFlock, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_AntiepidemicPlan\" (\"Id\",\"Name\",\"Vaccine\",\"PlanExecuteDate\",\"SheepFlock\",\"IsExcuted\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:name,:vaccine,:planExecuteDate,:sheepFlock,:isExcuted,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("vaccine", vaccine);
            pms.AddWithValue("planExecuteDate", planExecuteDate);
            pms.AddWithValue("sheepFlock", sheepFlock);
            pms.AddWithValue("isExcuted", false);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public List<AntiepidemicPlan> GetAntiepidemicPlan(AntiepidemicPlanFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY a.\"CreateTime\" desc) as \"rownum\" ,a.\"Id\",a.\"Name\",a.\"Vaccine\",a.\"PlanExecuteDate\",a.\"SheepFlock\",a.\"IsExcuted\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as\"OperatorName\",a.\"CreateTime\",a.\"Remark\" from \"T_AntiepidemicPlan\" a join \"T_Employee\" e on e.\"Id\"=a.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=a.\"OperatorId\"";

            string countSql = "select count(a.\"Id\") from \"T_AntiepidemicPlan\" a join \"T_Employee\" e on e.\"Id\"=a.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=a.\"OperatorId\"";

            return GetPagedData<AntiepidemicPlan, AntiepidemicPlanFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<AntiepidemicPlan> GetAntiepidemicPlan(AntiepidemicPlanFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY a.\"CreateTime\" desc) as \"rownum\" ,a.\"Id\",a.\"Name\",a.\"Vaccine\",a.\"PlanExecuteDate\",a.\"SheepFlock\",a.\"IsExcuted\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as\"OperatorName\",a.\"CreateTime\",a.\"Remark\" from \"T_AntiepidemicPlan\" a join \"T_Employee\" e on e.\"Id\"=a.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=a.\"OperatorId\"";

            return GetRuledRowsData<AntiepidemicPlan, AntiepidemicPlanFilter>(rowsCount, querySql, filter);
        }

        public AntiepidemicPlan GetAntiepidemicPlanById(string id)
        {
            string sql = "select a.\"Id\",a.\"Name\",a.\"Vaccine\",a.\"PlanExecuteDate\",a.\"SheepFlock\",a.\"IsExcuted\",a.\"PrincipalId\",a.\"OperatorId\",a.\"CreateTime\",a.\"Remark\" from \"T_AntiepidemicPlan\" a where a.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return GetData<AntiepidemicPlan>(sql, pms).FirstOrDefault();
        }

        public void UpdateAntiepidemicPlan(string name, string vaccine, DateTime planExecuteDate, string sheepFlock, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_AntiepidemicPlan\" SET \"Name\"=:name,\"Vaccine\"=:vaccine,\"PlanExecuteDate\"=:planExecuteDate,\"SheepFlock\"=:sheepFlock,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("vaccine", vaccine);
            pms.AddWithValue("planExecuteDate", planExecuteDate);
            pms.AddWithValue("sheepFlock", sheepFlock);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        [Transaction]
        public void ExecuteAntiepidemicPlan(string id, string planId, string name, string vaccine, DateTime executeDate, string effect, string sheepFlock, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "UPDATE \"T_AntiepidemicPlan\" SET \"IsExcuted\"=:isExcuted,\"ExcuteAntiepidemicId\"=:excuteAntiepidemicId where \"Id\"=:planId;INSERT into \"T_Antiepidemic\" (\"Id\",\"Name\",\"Vaccine\",\"ExecuteDate\",\"Effect\",\"SheepFlock\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:name,:vaccine,:executeDate,:effect,:sheepFlock,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("isExcuted", true);
            pms.AddWithValue("excuteAntiepidemicId", id);
            pms.AddWithValue("planId", planId);

            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("vaccine", vaccine);
            pms.AddWithValue("executeDate", executeDate);
            pms.AddWithValue("effect", effect);
            pms.AddWithValue("sheepFlock", sheepFlock);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void AddTreatment(string id, string sheepId, string symptom, DateTime startDate, string disease, string treatmentPlan, int treatmentDays, string effect, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Treatment\" (\"Id\",\"SheepId\",\"Symptom\",\"StartDate\",\"Disease\",\"TreatmentPlan\",\"TreatmentDays\",\"Effect\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:sheepId,:symptom,:startDate,:disease,:treatmentPlan,:treatmentDays,:effect,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("sheepId", sheepId);
            pms.AddWithValue("symptom", symptom);
            pms.AddWithValue("startDate", startDate);
            pms.AddWithValue("disease", disease);
            pms.AddWithValue("treatmentPlan", treatmentPlan);
            pms.AddWithValue("treatmentDays", treatmentDays);
            pms.AddWithValue("effect", effect);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public List<Treatment> GetTreatment(TreatmentFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY t.\"CreateTime\" desc) as \"rownum\" ,t.\"Id\",s.\"SerialNumber\",t.\"Symptom\",t.\"StartDate\",t.\"Disease\",t.\"TreatmentPlan\",t.\"TreatmentDays\",t.\"Effect\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as\"OperatorName\",t.\"CreateTime\",t.\"Remark\" from \"T_Treatment\" t  join \"T_Sheep\" s on t.\"SheepId\"=s.\"Id\" join \"T_Employee\" e on e.\"Id\"=t.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=t.\"OperatorId\"";

            string countSql = "select count(t.\"Id\") from \"T_Treatment\" t  join \"T_Sheep\" s on t.\"SheepId\"=s.\"Id\" join \"T_Employee\" e on e.\"Id\"=t.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=t.\"OperatorId\"";

            return GetPagedData<Treatment, TreatmentFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<Treatment> GetTreatment(TreatmentFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY t.\"CreateTime\" desc) as \"rownum\" ,t.\"Id\",s.\"SerialNumber\",t.\"Symptom\",t.\"StartDate\",t.\"Disease\",t.\"TreatmentPlan\",t.\"TreatmentDays\",t.\"Effect\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as\"OperatorName\",t.\"CreateTime\",t.\"Remark\" from \"T_Treatment\" t  join \"T_Sheep\" s on t.\"SheepId\"=s.\"Id\" join \"T_Employee\" e on e.\"Id\"=t.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=t.\"OperatorId\"";

            return GetRuledRowsData<Treatment, TreatmentFilter>(rowsCount, querySql, filter);
        }

        public Treatment GetTreatmentById(string id)
        {
            string sql = "select t.\"Id\",t.\"SheepId\",s.\"SerialNumber\",t.\"Symptom\",t.\"StartDate\",t.\"Disease\",t.\"TreatmentPlan\",t.\"TreatmentDays\",t.\"Effect\",t.\"PrincipalId\",t.\"OperatorId\",t.\"CreateTime\",t.\"Remark\" from \"T_Treatment\" as t join \"T_Sheep\" s on t.\"SheepId\"=s.\"Id\" where t.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return GetData<Treatment>(sql, pms).FirstOrDefault();
        }

        public void UpdateTreatment(string symptom, DateTime startDate, string disease, string treatmentPlan, int treatmentDays, string effect, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_Treatment\" SET \"Symptom\"=:symptom,\"StartDate\"=:startDate,\"Disease\"=:disease,\"TreatmentPlan\"=:treatmentPlan,\"TreatmentDays\"=:treatmentDays,\"Effect\"=:effect,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("symptom", symptom);
            pms.AddWithValue("startDate", startDate);
            pms.AddWithValue("disease", disease);
            pms.AddWithValue("treatmentPlan", treatmentPlan);
            pms.AddWithValue("treatmentDays", treatmentDays);
            pms.AddWithValue("effect", effect);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }


        #endregion
    }
}
