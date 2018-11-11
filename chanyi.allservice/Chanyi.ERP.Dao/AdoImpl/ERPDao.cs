using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spring.Data.Common;
using Spring.Data.Generic;

using Chanyi.Common.Domain;
using Chanyi.Doamin;
using Chanyi.ERP.Domain.Enums;
using Chanyi.ERP.Domain.Event;
using Chanyi.ERP.Domain.IDao;
using Spring.Transaction.Interceptor;

namespace Chanyi.ERP.Dao.AdoImpl
{
    public class ERPDao : AdoDaoSupport, IERPDao,
        IEventHandler<CreateAccountEvent>,
        IEventHandler<DeleteAccountEvent>,
        IEventHandler<UpdateAccountEvent>,

        IEventHandler<CreateBookkeepingEvent>,
        IEventHandler<CorrectBookkeepingEvent>,

        IEventHandler<CreateDepartmentEvent>,
        IEventHandler<UpdateDepartmentEvent>,
        IEventHandler<DeleteDepartmentEvent>,

        IEventHandler<CreateDutyEvent>,
        IEventHandler<UpdateDutyEvent>,
        IEventHandler<DeleteDutyEvent>,

        IEventHandler<CreateUserEvent>,
        IEventHandler<UpdateUserEvent>,
        IEventHandler<DeleteUserEvent>
    {
        #region Query
        public int GetAccountCount(string id)
        {
            string sql = "SELECT count(1) from t_account where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms));
        }

        public decimal GetLastBalance()
        {
            string sql = "select Balance from t_bookkeeping ORDER BY CreateTime DESC LIMIT 0,1";
            return Convert.ToDecimal(AdoTemplate.ExecuteScalar(CommandType.Text, sql));
        }

        public decimal GetBalanceById(string id)
        {
            string sql = "select balance from T_bookkeeping where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            object obj = AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
            return Convert.ToDecimal(obj);
        }

        public int GetDepartmentCount(string id)
        {
            string sql = "select count(1) from T_department where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms));
        }

        public int GetDutyCount(string id)
        {
            string sql = "select count(1) from T_duty where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms));
        }

        public int GetUserCount(string id)
        {
            string sql = "select count(1) from T_user where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms));
        }

        public int GetUserTotalCount()
        {
            string sql = "select count(1) from T_user";
            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql));
        }

        public int GetBookkeepingCount(string id)
        {
            string sql = "select count(1) from T_bookkeeping where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms));
        }

        public BadBookkeeping GetBadBookkeeping(string id)
        {
            string sql = "select amount,direction,status from T_bookkeeping where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);

            return AdoTemplate.QueryWithRowMapperDelegate<BadBookkeeping>(CommandType.Text, sql, (reader, rowNum) => new BadBookkeeping { Amount = reader.GetDecimal(reader.GetOrdinal("amount")), Direction = (BookkeepingDirectionEnum)reader.GetInt32(reader.GetOrdinal("direction")), Status=(BookkeepingStatusEnum)reader.GetInt32(reader.GetOrdinal("status")) }, pms).FirstOrDefault();
        }

        #endregion

        #region Handle
        public void Handle(CreateAccountEvent evt)
        {
            string sql = "insert into t_account VALUES(@Id,@Name,@Account,@Password,@CreaterId,@CreateTime)";

            IDbParameters parameters = AdoTemplate.CreateDbParameters();
            parameters.AddWithValue("Id", evt.Id);
            parameters.AddWithValue("Name", evt.Name);
            parameters.AddWithValue("Account", evt.Account);
            parameters.AddWithValue("Password", evt.Password);
            parameters.AddWithValue("CreaterId", evt.CreaterId);
            parameters.AddWithValue("CreateTime", evt.CreateTime);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        public void Handle(DeleteAccountEvent evt)
        {
            string sql = "delete from t_account where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", evt.Id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void Handle(UpdateAccountEvent evt)
        {
            string sql = "update T_account set name=@name, account=@account, password=@password where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", evt.Name);
            pms.AddWithValue("account", evt.Account);
            pms.AddWithValue("password", evt.Password);
            pms.AddWithValue("id", evt.Id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void Handle(CreateBookkeepingEvent evt)
        {
            string sql = @"INSERT INTO t_bookkeeping ( abstract, amount, balance, createrId, createTime, direction, id, remark, STATUS, time, voucherNum, voucherType ) VALUES ( @abstract ,@amount ,@balance ,@createrId ,@createTime ,@direction ,@id ,@remark ,@STATUS ,@time ,@voucherNum ,@voucherType )";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("abstract", evt.Abstract);
            pms.AddWithValue("amount", evt.Amount);
            pms.AddWithValue("balance", evt.Balance);
            pms.AddWithValue("createrId", evt.CreaterId);
            pms.AddWithValue("createTime", evt.CreateTime);
            pms.AddWithValue("direction", evt.Direction);
            pms.AddWithValue("id", evt.Id);
            pms.AddWithValue("remark", evt.Remark);
            pms.AddWithValue("STATUS", evt.Status);
            pms.AddWithValue("time", evt.Time);
            pms.AddWithValue("voucherNum", evt.VoucherNum);
            pms.AddWithValue("voucherType", evt.VoucherType);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        [Transaction]
        public void Handle(CorrectBookkeepingEvent evt)
        {
            //更新错账
            string sqlUpdate = "update t_bookkeeping set Status=@Status,AbandonReason=@Reason WHERE Id=@OldId";
            IDbParameters pmUpdate = AdoTemplate.CreateDbParameters();
            pmUpdate.AddWithValue("Status", evt.OldStatus);
            pmUpdate.AddWithValue("Reason", evt.AbandonReason);
            pmUpdate.AddWithValue("OldId", evt.OldId);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sqlUpdate, pmUpdate);

            //回滚错账
            string rbkId = Guid.NewGuid().ToString();
            string sqlRbk = "insert into t_bookkeeping ( Id, Time, VoucherType, VoucherNum, Abstract, Amount,Direction,Balance,Status, CreaterId, CreateTime, Remark ) SELECT '" + rbkId + "', Time, VoucherType, VoucherNum, Abstract, Amount,Direction,Balance,Status, CreaterId, now(), Remark from t_bookkeeping WHERE id =@Id";
            IDbParameters pmRbk = AdoTemplate.CreateDbParameters();
            pmRbk.AddWithValue("Id", evt.OldId);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sqlRbk, pmRbk);

            string sqlUpdateRbk = "update t_bookkeeping set Direction=@Direction,Balance=@Balance,ReferenceId=@ReferenceId,Status=@Status where Id=@Id";
            IDbParameters pmUpdateRbk = AdoTemplate.CreateDbParameters();
            pmUpdateRbk.AddWithValue("Direction", evt.RbkDirection);
            pmUpdateRbk.AddWithValue("Balance", evt.RbkBalance);
            pmUpdateRbk.AddWithValue("ReferenceId", evt.OldId);
            pmUpdateRbk.AddWithValue("Status", evt.RbkStatus);
            pmUpdateRbk.AddWithValue("Id", rbkId);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sqlUpdateRbk, pmUpdateRbk);

            //记录新账
            string sqlNew = "insert into t_bookkeeping (Id,Time,VoucherType,VoucherNum,Abstract,Amount,Direction,Balance,CreaterId,CreateTime,ReferenceId,Status,Remark) VALUES(@Id,@Time,@VoucherType,@VoucherNum,@Abstract,@Amount,@Direction,@Balance,@CreaterId,@CreateTime,@ReferenceId,@Status,@Remark)";
            IDbParameters pmUpdateNew = AdoTemplate.CreateDbParameters();
            pmUpdateNew.AddWithValue("Id", evt.NewId);
            pmUpdateNew.AddWithValue("Time", evt.Time);
            pmUpdateNew.AddWithValue("VoucherType", evt.NewVoucherType);
            pmUpdateNew.AddWithValue("VoucherNum", evt.NewVoucherNum);
            pmUpdateNew.AddWithValue("Abstract", evt.NewAbstract);
            pmUpdateNew.AddWithValue("Amount", evt.NewAmount);
            pmUpdateNew.AddWithValue("Direction", evt.NewDirection);
            pmUpdateNew.AddWithValue("Balance", evt.NewBalance);
            pmUpdateNew.AddWithValue("CreaterId", evt.NewCreaterId);
            pmUpdateNew.AddWithValue("CreateTime", evt.CreateTime);
            pmUpdateNew.AddWithValue("ReferenceId", evt.OldId);
            pmUpdateNew.AddWithValue("Status", evt.NewStatus);
            pmUpdateNew.AddWithValue("Remark", evt.NewRemark);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sqlNew, pmUpdateNew);
        }

        public void Handle(CreateDepartmentEvent evt)
        {
            string sql = "insert into t_department(createrid,createtime,`desc`,id,isenabled,name) values(@createrid,@createtime,@desc,@id,@isenabled,@name)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();

            pms.AddWithValue("createrid", evt.CreaterId);
            pms.AddWithValue("createtime", evt.CreateTime);
            pms.AddWithValue("desc", evt.Desc);
            pms.AddWithValue("id", evt.Id);
            pms.AddWithValue("isenabled", evt.IsEnabled);
            pms.AddWithValue("name", evt.Name);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void Handle(UpdateDepartmentEvent evt)
        {
            string sql = "update t_department set `desc`=@desc,isenabled=@isenabled,name=@name where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("desc", evt.Desc);
            pms.AddWithValue("isenabled", evt.IsEnabled);
            pms.AddWithValue("name", evt.Name);
            pms.AddWithValue("id", evt.Id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public void Handle(DeleteDepartmentEvent evt)
        {
            string sql = "delete from t_department where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", evt.Id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void Handle(CreateDutyEvent evt)
        {
            string sql = "insert into T_duty (createrid,createtime,departmentid,`desc`,id,isenabled,name) values(@createrid,@createtime,@departmentid,@desc,@id,@isenabled,@name)";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("createrid", evt.CreaterId);
            pms.AddWithValue("createtime", evt.CreateTime);
            pms.AddWithValue("departmentid", evt.DepartmentId);
            pms.AddWithValue("desc", evt.Desc);
            pms.AddWithValue("id", evt.Id);
            pms.AddWithValue("isenabled", evt.IsEnabled);
            pms.AddWithValue("name", evt.Name);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void Handle(UpdateDutyEvent evt)
        {
            string sql = "update T_duty set departmentid=@departmentid,`desc`=@desc,isenabled=@isenabled,name=@name where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("departmentid", evt.DepartmentId);
            pms.AddWithValue("desc", evt.Desc);
            pms.AddWithValue("isenabled", evt.IsEnabled);
            pms.AddWithValue("name", evt.Name);
            pms.AddWithValue("id", evt.Id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void Handle(DeleteDutyEvent evt)
        {
            string sql = "delete from t_duty where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", evt.Id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void Handle(CreateUserEvent evt)
        {
            string sql = "INSERT INTO T_user ( address, birthday, createrid, createtime, degree, employeenum, entrytime, gender, id, idnum, nationid, PASSWORD, phonenum, realname, STATUS, username ) VALUES ( @address ,@birthday ,@createrid ,@createtime ,@degree ,@employeenum ,@entrytime ,@gender ,@id ,@idnum ,@nationid ,@PASSWORD ,@phonenum ,@realname ,@STATUS ,@username )";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("address", evt.Address);
            pms.AddWithValue("birthday", evt.Birthday);
            pms.AddWithValue("createrid", evt.CreaterId);
            pms.AddWithValue("createtime", evt.CreateTime);
            pms.AddWithValue("degree", evt.Degree);
            pms.AddWithValue("employeenum", evt.EmployeeNum);
            pms.AddWithValue("entrytime", evt.EntryTime);
            pms.AddWithValue("gender", evt.Gender);
            pms.AddWithValue("id", evt.Id);
            pms.AddWithValue("idnum", evt.IdNum);
            pms.AddWithValue("nationid", evt.NationId);
            pms.AddWithValue("PASSWORD", evt.Password);
            pms.AddWithValue("phonenum", evt.PhoneNum);
            pms.AddWithValue("realname", evt.RealName);
            pms.AddWithValue("STATUS", evt.Status);
            pms.AddWithValue("username", evt.UserName);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public void Handle(UpdateUserEvent evt)
        {
            string sql = "update T_user set address=@address,  birthday=@birthday,   degree=@degree,   entrytime=@entrytime,  gender=@gender,  idnum=@idnum,  nationid=@nationid,  PASSWORD=@PASSWORD,  phonenum=@phonenum,  realname=@realname,  STATUS=@STATUS,  username=@username where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("address", evt.Address);
            pms.AddWithValue("birthday", evt.Birthday);
            pms.AddWithValue("degree", evt.Degree);
            pms.AddWithValue("entrytime", evt.EntryTime);
            pms.AddWithValue("gender", evt.Gender);
            pms.AddWithValue("idnum", evt.IdNum);
            pms.AddWithValue("nationid", evt.NationId);
            pms.AddWithValue("PASSWORD", evt.Password);
            pms.AddWithValue("phonenum", evt.PhoneNum);
            pms.AddWithValue("realname", evt.RealName);
            pms.AddWithValue("STATUS", evt.Status);
            pms.AddWithValue("username", evt.UserName);
            pms.AddWithValue("id", evt.Id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public void Handle(DeleteUserEvent evt)
        {
            string sql = "delete from t_user where id=@id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", evt.Id);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        #endregion

    }
}
