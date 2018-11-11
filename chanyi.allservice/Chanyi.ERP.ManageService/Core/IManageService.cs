using Chanyi.Common.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.ManageService.Core
{
    [ServiceContract]
    public interface IManageService
    {
        #region Account

        /// <summary>
        /// 创建账户
        /// </summary>
        /// <param name="name">账户名称</param>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <param name="createrId">创建人</param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<string> CreateAccount(string name, string account, string password, string createrId);

        /// <summary>
        /// 更新账户
        /// </summary>
        /// <param name="name">账户名称</param>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<bool> UpdateAccount(string name, string account, string password, string id);

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<bool> DeleteAccount(string id);

        #endregion

        #region Bookkeeping

        /// <summary>
        /// 创建账目
        /// </summary>
        /// <param name="time">账发生日期</param>
        /// <param name="voucherTypeId">凭证种类ID</param>
        /// <param name="voucherNum">凭证号数</param>
        /// <param name="abst">摘要：abstract</param>
        /// <param name="amount">金额</param>
        /// <param name="direction">借贷方向ID</param>
        /// <param name="balance">余额</param>
        /// <param name="remark">备注</param>
        /// <param name="createrId">创建者ID</param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<string> CreateBookkeeping(DateTime time, string voucherTypeId, string voucherNum, string abst, decimal amount, int direction, string remark, string createrId);


        /// <summary>
        /// 修改账目
        /// </summary>
        /// <param name="oldId">原始账目ID</param>
        /// <param name="abandonReason">废弃原因</param>
        /// <param name="time">账发生日期</param>
        /// <param name="voucherTypeId">凭证种类ID</param>
        /// <param name="voucherNum">凭证号数</param>
        /// <param name="abst">摘要：abstract</param>
        /// <param name="amount">金额</param>
        /// <param name="direction">借贷方向ID</param>
        /// <param name="balance">余额</param>
        /// <param name="remark">备注</param>
        /// <param name="createrId">创建者ID</param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<bool> CorrectBookkeeping(string oldId, string abandonReason, DateTime time, string voucherTypeId, string voucherNum, string abst, decimal amount, int direction, string remark, string createrId);


        #endregion

        #region Department

        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="name">部门名称</param>
        /// <param name="desc">部门功能描述</param>
        /// <param name="isEnabled">是否启用</param>
        /// <param name="createrId">创建者ID</param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<string> CreateDepartment(string name, string desc, string createrId);

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="name">部门名称</param>
        /// <param name="desc">部门功能描述</param>
        /// <param name="isEnabled">是否启用</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<bool> UpdateDepartment(string name, string desc, bool isEnabled, string id);

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<bool> DeleteDepartment(string id);

        #endregion

        #region Duty

        /// <summary>
        /// 创建职务
        /// </summary>
        /// <param name="name">职务名称</param>
        /// <param name="desc">描述</param>
        /// <param name="departmentId">部门编号</param>
        /// <param name="createrId">创建者</param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<string> CreateDuty(string name, string desc, string departmentId, string createrId);

        /// <summary>
        /// 修改职务
        /// </summary>
        /// <param name="name">职务名称</param>
        /// <param name="desc">描述</param>
        /// <param name="departmentId">部门编号</param>
        /// <param name="isEnabled">是否启用</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<bool> UpdateDuty(string name, string desc, string departmentId, bool isEnabled, string id);

        /// <summary>
        /// 删除职务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<bool> DeleteDuty(string id);

        #endregion

        #region User

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="realName">真是姓名</param>
        /// <param name="password">密码</param>
        /// <param name="idNum">身份证号</param>
        /// <param name="phoneNum">手机号</param>
        /// <param name="gender">性别</param>
        /// <param name="adress">住址</param>
        /// <param name="degree">学历</param>
        /// <param name="entryTime">入职时间</param>
        /// <param name="nationId">民族编号</param>
        /// <param name="birthday">出生日期</param>
        /// <param name="createrId">创建者</param>
        /// <param name="status">在职状态</param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<string> CreateUser(string userName, string realName, string password, string idNum, string phoneNum, int gender, string address, string degree, DateTime entryTime, string nationId, DateTime birthday,string createrId, int status);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="realName">真是姓名</param>
        /// <param name="password">密码</param>
        /// <param name="idNum">身份证号</param>
        /// <param name="phoneNum">手机号</param>
        /// <param name="gender">性别</param>
        /// <param name="adress">住址</param>
        /// <param name="degree">学历</param>
        /// <param name="entryTime">入职时间</param>
        /// <param name="nationId">民族编号</param>
        /// <param name="birthday">出生日期</param>
        /// <param name="status">在职状态</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<bool> UpdateUser(string userName, string realName, string password, string idNum, string phoneNum, int gender, string address, string degree, DateTime entryTime, string nationId, DateTime birthday, int status, string id);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResult<bool> DeleteUser(string id);

        #endregion
    }
}
