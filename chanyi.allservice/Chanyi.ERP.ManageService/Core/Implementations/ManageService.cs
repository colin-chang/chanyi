using Chanyi.Common.Domain;
using Chanyi.Common.Service;
using Chanyi.Doamin;
using Chanyi.ERP.Domain.Enums;
using Chanyi.ERP.Domain.IDao;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.ManageService.Core.Implementations
{
    [ServiceBehavior(UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ManageService : IManageService
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IERPDao ERPDao { get; set; }

        public Common.Service.ServiceResult<string> CreateAccount(string name, string account, string password, string createrId)
        {
            try
            {
                string id = new Accounts().Create(name, account, password, createrId);
                return new ServiceResult<string>(id);
            }

            catch (DomainException domainEx)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, 2, domainEx.Message);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.Unknown, ex.Message);
            }
        }

        public ServiceResult<bool> UpdateAccount(string name, string account, string password, string id)
        {
            try
            {
                if (ERPDao.GetAccountCount(id) <= 0)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "记录不存在");

                new Accounts().Update(name, account, password, id);
                return new ServiceResult<bool>(true);
            }

            catch (DomainException domainEx)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, 2, domainEx.Message);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.Unknown, ex.Message);
            }
        }

        public ServiceResult<bool> DeleteAccount(string id)
        {
            try
            {
                if (ERPDao.GetAccountCount(id) <= 0)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "记录不存在");

                new Accounts().Delete(id);
                return new ServiceResult<bool>(true);
            }

            catch (DomainException domainEx)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, 2, domainEx.Message);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.Unknown, ex.Message);
            }
        }

        public ServiceResult<string> CreateBookkeeping(DateTime time, string voucherTypeId, string voucherNum, string abst, decimal amount, int direction, string remark, string createrId)
        {
            try
            {
                decimal balance = ERPDao.GetLastBalance();
                balance = ((BookkeepingDirectionEnum)direction) == BookkeepingDirectionEnum.Borrow ? balance + amount : balance - amount;

                string id = new Bookkeeping().Create(time, voucherTypeId, voucherNum, abst, amount, direction, balance, remark, createrId);
                return new ServiceResult<string>(id);
            }

            catch (DomainException domainEx)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, 2, domainEx.Message);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.Unknown, ex.Message);
            }
        }

        public ServiceResult<bool> CorrectBookkeeping(string oldId, string abandonReason, DateTime time, string voucherTypeId, string voucherNum, string abst, decimal amount, int direction, string remark, string createrId)
        {
            try
            {
                if (ERPDao.GetBookkeepingCount(oldId) <= 0)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "账目不存在");

                BadBookkeeping bbk = ERPDao.GetBadBookkeeping(oldId);
                if (bbk.Status != BookkeepingStatusEnum.Normal)
                    return new ServiceResult<bool>(false, ResultStatus.ERROR, "当前账目不可修改");

                decimal lstB = ERPDao.GetLastBalance();
                decimal rbkB = bbk.Direction == BookkeepingDirectionEnum.Borrow ? lstB - bbk.Amount : lstB + bbk.Amount;
                decimal newB = direction == (int)BookkeepingDirectionEnum.Borrow ? rbkB + amount : rbkB - amount;

                new Bookkeeping().Correct(remark, oldId, createrId, voucherTypeId, rbkB, bbk.Direction==BookkeepingDirectionEnum.Borrow?BookkeepingDirectionEnum.Loan:BookkeepingDirectionEnum.Borrow, abandonReason, abst, amount, newB, (BookkeepingDirectionEnum)direction, voucherNum, bbk.Amount, time);

                return new ServiceResult<bool>(true);
            }

            catch (DomainException domainEx)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, 2, domainEx.Message);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.Unknown, ex.Message);
            }
        }

        public ServiceResult<string> CreateDepartment(string name, string desc, string createrId)
        {
            try
            {
                string id = new Department().Create(name, desc, createrId);
                return new ServiceResult<string>(id);
            }

            catch (DomainException domainEx)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, 2, domainEx.Message);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.Unknown, ex.Message);
            }
        }

        public ServiceResult<bool> UpdateDepartment(string name, string desc, bool isEnabled, string id)
        {
            try
            {
                if (ERPDao.GetDepartmentCount(id) <= 0)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "记录不存在");
                new Department().Update(name, desc, isEnabled, id);

                return new ServiceResult<bool>(true);
            }
            catch (DomainException domainEx)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, domainEx.Message);

            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, ex.Message);
            }
        }

        public ServiceResult<bool> DeleteDepartment(string id)
        {
            try
            {
                if (ERPDao.GetDepartmentCount(id) <= 0)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "记录不存在");
                new Department().Delete(id);

                return new ServiceResult<bool>(true);
            }
            catch (DomainException domainEx)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, domainEx.Message);

            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, ex.Message);
            }
        }

        public ServiceResult<string> CreateDuty(string name, string desc, string departmentId, string createrId)
        {
            try
            {
                string id = new Duty().Create(name, desc, departmentId, createrId);

                return new ServiceResult<string>(id);
            }
            catch (DomainException domainEx)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, domainEx.Message);

            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, ex.Message);
            }
        }

        public ServiceResult<bool> UpdateDuty(string name, string desc, string departmentId, bool isEnabled, string id)
        {
            try
            {
                if (ERPDao.GetDutyCount(id) <= 0)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "记录不存在");
                new Duty().Update(name, desc, departmentId, isEnabled, id);

                return new ServiceResult<bool>(true);
            }
            catch (DomainException domainEx)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, domainEx.Message);

            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, ex.Message);
            }
        }

        public ServiceResult<bool> DeleteDuty(string id)
        {
            try
            {
                if (ERPDao.GetDutyCount(id) <= 0)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "记录不存在");
                new Duty().Delete(id);

                return new ServiceResult<bool>(true);
            }
            catch (DomainException domainEx)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, domainEx.Message);

            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, ex.Message);
            }
        }

        public ServiceResult<string> CreateUser(string userName, string realName, string password, string idNum, string phoneNum, int gender, string address, string degree, DateTime entryTime, string nationId, DateTime birthday, string createrId, int status)
        {
            try
            {
                string employeeNum = DateTime.Now.Date.ToString() + ERPDao.GetUserTotalCount();

                string id = new User().Create(userName, realName, password, idNum, phoneNum, gender, address, employeeNum, degree, entryTime, nationId, birthday, createrId, status);

                return new ServiceResult<string>(id);
            }
            catch (DomainException domainEx)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, domainEx.Message);

            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, ex.Message);
            }
        }

        public ServiceResult<bool> UpdateUser(string userName, string realName, string password, string idNum, string phoneNum, int gender, string address, string degree, DateTime entryTime, string nationId, DateTime birthday, int status, string id)
        {
            try
            {
                if (ERPDao.GetUserCount(id) <= 0)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "记录不存在");
                new User().Update(userName, realName, password, idNum, phoneNum, gender, address, degree, entryTime, nationId, birthday, status, id);

                return new ServiceResult<bool>(true);
            }
            catch (DomainException domainEx)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, domainEx.Message);

            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, ex.Message);
            }
        }

        public ServiceResult<bool> DeleteUser(string id)
        {
            try
            {
                if (ERPDao.GetUserCount(id) <= 0)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "记录不存在");
                new User().Delete(id);

                return new ServiceResult<bool>(true);
            }
            catch (DomainException domainEx)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, domainEx.Message);

            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, ex.Message);
            }
        }
    }
}
