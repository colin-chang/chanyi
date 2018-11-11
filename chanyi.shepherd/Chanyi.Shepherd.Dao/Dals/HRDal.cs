using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Spring.Data.Common;
using Spring.Transaction.Interceptor;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.Filter.HR;
using Chanyi.Shepherd.QueryModel.Model.HR;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region HR

        public int GetEmployeeCountByName(string name)
        {
            string sql = "select count(\"Id\") from \"T_Employee\" where \"Name\"=:name";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            return GetDataCount(sql, pms);
        }

        public int GetEmployeeCountBySerialNum(string serNum)
        {
            string sql = "select count(\"Id\") from \"T_Employee\" where \"SerialNum\"=:serNum";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("serNum", serNum);
            return GetDataCount(sql, pms);
        }

        public void AddEmployee(string id, string name, GenderEnum gender, string idNum, DateTime entryDate, decimal salary, string serialNum, string dutyId, EmployeeStatusEnum status, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Employee\" (\"Id\",\"Name\",\"Gender\",\"IdNum\",\"EntryDate\",\"Salary\",\"SerialNum\",\"DutyId\",\"Status\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:name,:gender,:idNum,:entryDate,:salary,:serialNum,:dutyId,:status,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("gender", (int)gender);
            pms.AddWithValue("idNum", idNum);
            pms.AddWithValue("entryDate", entryDate);
            pms.AddWithValue("salary", salary);
            pms.AddWithValue("serialNum", serialNum);
            pms.AddWithValue("dutyId", dutyId);
            pms.AddWithValue("status", (int)status);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public List<Employee> GetEmployee(EmployeeFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY e.\"EntryDate\" DESC)  \"rownum\",e.\"Id\",e.\"Name\",e.\"Gender\",e.\"IdNum\",e.\"EntryDate\",e.\"Salary\",e.\"SerialNum\",d.\"Name\" as \"DutyName\",e.\"Status\",e.\"Salary\",e.\"CreateTime\", p.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"Remark\" from \"T_Employee\" e left join \"T_Duty\" d on d.\"Id\"=e.\"DutyId\" left JOIN \"T_Employee\" p on p.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            string countSql = "select count(e.\"Id\") from \"T_Employee\" e left join \"T_Duty\" d on d.\"Id\"=e.\"DutyId\" left JOIN \"T_Employee\" p on p.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            return GetPagedData<Employee, EmployeeFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<Employee> GetEmployee(EmployeeFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY e.\"EntryDate\" DESC)  \"rownum\",e.\"Id\",e.\"Name\",e.\"Gender\",e.\"IdNum\",e.\"EntryDate\",e.\"Salary\",e.\"SerialNum\",d.\"Name\" as \"DutyName\",e.\"Status\",e.\"Salary\",e.\"CreateTime\", p.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"Remark\" from \"T_Employee\" e left join \"T_Duty\" d on d.\"Id\"=e.\"DutyId\" left JOIN \"T_Employee\" p on p.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            return GetRuledRowsData<Employee, EmployeeFilter>(rowsCount, querySql, filter);
        }
        public Employee GetEmployeeById(string id)
        {
            string sql = "select e.\"CreateTime\",e.\"DutyId\",e.\"EntryDate\",e.\"Gender\",e.\"Id\",e.\"IdNum\",e.\"Name\",e.\"OperatorId\",e.\"PrincipalId\",e.\"Remark\",e.\"Salary\",e.\"SerialNum\",e.\"Status\",u.\"UserName\" AS \"OperatorName\", e.\"Name\" AS \"PrincipalName\",d.\"Name\" AS \"DutyName\"  from \"T_Employee\" e join \"T_User\" u ON u.\"Id\" = e.\"OperatorId\" left join \"T_Employee\" employee ON employee.\"Id\" = e.\"PrincipalId\" left join \"T_Duty\" d ON d.\"Id\" = e.\"DutyId\" where e.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("Id", id);

            return GetData<Employee>(sql, pms).FirstOrDefault();
        }

        public Employee GetEmployeeByName(string name)
        {
            string sql = "select \"Id\"  from \"T_Employee\" e where e.\"Name\"=:name";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);

            return GetData<Employee>(sql, pms).FirstOrDefault();
        }
        public Employee GetEmployeeBySerialNum(string serNum)
        {
            string sql = "select \"Id\"  from \"T_Employee\" e where e.\"SerialNum\"=:serNum";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("serNum", serNum);

            return GetData<Employee>(sql, pms).FirstOrDefault();
        }

        public void UpdateEmployee(string name, GenderEnum gender, string idNum, DateTime entryDate, decimal salary, string serialNum, string dutyId, EmployeeStatusEnum status, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_Employee\" SET \"Name\"=:name,\"Gender\"=:gender,\"IdNum\"=:idNum,\"EntryDate\"=:entryDate,\"Salary\"=:salary,\"SerialNum\"=:serialNum,\"DutyId\"=:dutyId,\"Status\"=:status,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            pms.AddWithValue("gender", (int)gender);
            pms.AddWithValue("idNum", idNum);
            pms.AddWithValue("entryDate", entryDate);
            pms.AddWithValue("salary", salary);
            pms.AddWithValue("serialNum", serialNum);
            pms.AddWithValue("dutyId", dutyId);
            pms.AddWithValue("status", (int)status);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        [Transaction]
        public void AddQuit(string id, string employeeId, string reason, DateTime quitDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Quit\" (\"Id\",\"EmployeeId\",\"Reason\",\"QuitDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:employeeId,:reason,:quitDate,:principalId,:operatorId,:createTime,:remark);UPDATE \"T_Employee\" SET \"Status\"=:status where  \"Id\"=:eid";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("employeeId", employeeId);
            pms.AddWithValue("reason", reason);
            pms.AddWithValue("quitDate", quitDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("status", (int)EmployeeStatusEnum.Dimission);
            pms.AddWithValue("eid", employeeId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }
        public List<Quit> GetQuit(QuitFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = "SELECT \"row_number\" () OVER (ORDER BY q.\"CreateTime\" DESC) \"rownum\", q.\"Id\", q.\"EmployeeId\", eq.\"Name\" AS \"EmployeeName\", q.\"QuitDate\", q.\"Reason\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", q.\"CreateTime\", q.\"Remark\" FROM \"T_Quit\" q JOIN \"T_Employee\" eq ON eq.\"Id\" = q.\"EmployeeId\" JOIN \"T_Employee\" e ON e.\"Id\" = q.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = q.\"OperatorId\" ";

            string countSql = "select count(q.\"Id\") FROM \"T_Quit\" q JOIN \"T_Employee\" eq ON eq.\"Id\" = q.\"EmployeeId\" JOIN \"T_Employee\" e ON e.\"Id\" = q.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = q.\"OperatorId\"";
            return GetPagedData<Quit, QuitFilter>(pageIndex, pageSize, out totalCount, countSql, sql, filter);
        }
        public List<Quit> GetQuit(QuitFilter filter, int rowsCount)
        {
            string sql = "SELECT \"row_number\" () OVER (ORDER BY q.\"CreateTime\" DESC) \"rownum\", q.\"Id\", q.\"EmployeeId\", eq.\"Name\" AS \"EmployeeName\", q.\"QuitDate\", q.\"Reason\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", q.\"CreateTime\", q.\"Remark\" FROM \"T_Quit\" q JOIN \"T_Employee\" eq ON eq.\"Id\" = q.\"EmployeeId\" JOIN \"T_Employee\" e ON e.\"Id\" = q.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = q.\"OperatorId\"";

            return GetRuledRowsData<Quit, QuitFilter>(rowsCount, sql, filter);
        }

        public int GetDutyCountByName(string name)
        {
            string sql = "select * from \"T_Duty\"  where \"Name\"=:name";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);

            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms));
        }

        public void AddDuty(string id, string name, string description, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Duty\" (\"Id\",\"Name\",\"Description\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:name,:description,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("description", description);
            pms.AddWithValue("principalId", operatorId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public List<User> GetUser(int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY u.\"Id\") as \"rownum\",u.\"Id\",u.\"UserName\",u.\"IsEnabled\",u.\"CreateTime\",u.\"Remark\",e.\"Name\" as \"PrincipalName\",usr.\"UserName\" as \"OperatorName\" from \"T_User\" u left join \"T_Employee\" e on e.\"Id\"=u.\"PrincipalId\" left  join \"T_User\" usr on usr.\"Id\"=u.\"OperatorId\"";

            string countSql = "select count(u.\"Id\") from \"T_User\" u left  join \"T_Employee\" e on e.\"Id\"=u.\"PrincipalId\" left  join \"T_User\" usr on usr.\"Id\"=u.\"OperatorId\"";

            return GetPagedData<User, UserFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, null);
        }
        public List<User> GetUser(int rowsCount)
        {
            string querySql = "select \"row_number\"() OVER(ORDER BY u.\"Id\") as \"rownum\",u.\"Id\",u.\"UserName\",u.\"IsEnabled\",u.\"CreateTime\",u.\"Remark\",e.\"Name\" as \"PrincipalName\",usr.\"UserName\" as \"OperatorName\" from \"T_User\" u left  join \"T_Employee\" e on e.\"Id\"=u.\"PrincipalId\" left  join \"T_User\" usr on usr.\"Id\"=u.\"OperatorId\"";
            return GetRuledRowsData<User, UserFilter>(rowsCount, querySql, null);
        }
        public User GetUserById(string id)
        {
            string sql = "select u.\"Id\",u.\"UserName\",u.\"PrincipalId\",u.\"OperatorId\",u.\"CreateTime\",u.\"IsEnabled\",u.\"Remark\" from \"T_User\" as u  WHERE \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return GetData<User>(sql, pms).FirstOrDefault();
        }

        public void UpdateUser(bool isEnabled, string remark, string id)
        {
            string sql = "UPDATE \"T_User\" SET \"IsEnabled\"=:isEnabled,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("isEnabled", isEnabled);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public List<Cooperater> GetCooperater(CooperaterFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY C .\"Id\") AS \"rownum\", C .\"Id\", C .\"Name\", C .\"Department\", C .\"ContactInfo\", C .\"CreateTime\", C .\"Remark\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\" FROM \"T_Cooperater\" C JOIN \"T_Employee\" e ON e.\"Id\" = C .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = C .\"OperatorId\"";

            string countSql = "select count(c.\"Id\") from \"T_Cooperater\" C JOIN \"T_Employee\" e ON e.\"Id\" = C .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = C .\"OperatorId\"";

            return GetPagedData<Cooperater, CooperaterFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<Cooperater> GetCooperater(CooperaterFilter filter, int rowsCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY C .\"Id\") AS \"rownum\", C .\"Id\", C .\"Name\", C .\"Department\", C .\"ContactInfo\", C .\"CreateTime\", C .\"Remark\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\" FROM \"T_Cooperater\" C JOIN \"T_Employee\" e ON e.\"Id\" = C .\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = C .\"OperatorId\"";

            return GetRuledRowsData<Cooperater, CooperaterFilter>(rowsCount, querySql, filter);
        }
        public Cooperater GetCooperaterById(string id)
        {
            string sql = "select c.\"Id\",c.\"Name\",c.\"Department\",c.\"ContactInfo\",c.\"PrincipalId\",c.\"Remark\" from \"T_Cooperater\" c  where c.\"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            return GetData<Cooperater>(sql, pms).FirstOrDefault();
        }

        public void UpdatePurchaser(string name, string department, string contactInfo, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_Cooperater\" SET \"Name\"=:name,\"Department\"=:department,\"ContactInfo\"=:contactInfo,\"PrincipalId\"=:principalId,\"Remark\"=:remark where  \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            pms.AddWithValue("department", department);
            pms.AddWithValue("contactInfo", contactInfo);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        [Transaction]
        public void AddPurchaser(string id, string name, string department, string contactInfo, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Purchaser\" (\"CooperaterId\")VALUES(:cooperaterId);INSERT into \"T_Cooperater\" (\"Id\",\"Name\",\"Department\",\"ContactInfo\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:name,:department,:contactInfo,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("cooperaterId", id);

            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("department", department);
            pms.AddWithValue("contactInfo", contactInfo);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        #endregion
    }
}
