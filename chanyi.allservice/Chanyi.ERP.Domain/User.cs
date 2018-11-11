using Chanyi.Common.Domain;
using Chanyi.ERP.Domain.Enums;
using Chanyi.ERP.Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Doamin
{
    public class User
    {
        public string Id { get; private set; }

        public string UserName { get; private set; }

        public string RealName { get; private set; }

        public string Password { get; private set; }

        public string IdNum { get; private set; }

        public string PhoneNum { get; private set; }

        public int Gender { get; private set; }

        public string Address { get; private set; }

        public string Degree { get; private set; }

        public DateTime EntryTime { get; private set; }

        public string NationId { get; private set; }

        public DateTime Birthday { get; private set; }

        public string EmployeeNum { get; private set; }

        public string CreaterId { get; private set; }

        public int Status { get; private set; }

        public DateTime CreateTime { get; private set; }

        public string Create(string userName, string realName, string password, string idNum, string phoneNum, int gender, string address, string employeeNum, string degree, DateTime entryTime, string nationId, DateTime birthday, string createrId, int status)
        {
            string id = Guid.NewGuid().ToString();

            DomainHelper.Publish(new CreateUserEvent() { Address = address, Birthday = birthday, CreaterId = createrId, CreateTime = DateTime.Now, Degree = degree, EmployeeNum = employeeNum, EntryTime = entryTime, Gender = gender, Id = id, IdNum = idNum, NationId = nationId, Password = password, PhoneNum = phoneNum, RealName = realName, Status = status, UserName = userName });

            return id;
        }

        public void Update(string userName, string realName, string password, string idNum, string phoneNum, int gender, string address, string degree, DateTime entryTime, string nationId, DateTime birthday, int status, string id)
        {
            DomainHelper.Publish(new UpdateUserEvent() { Address = address, Birthday = birthday, Degree = degree, EntryTime = entryTime, Gender = gender, Id = id, IdNum = idNum, NationId = nationId, Password = password, PhoneNum = phoneNum, RealName = realName, Status = status, UserName = userName });
        }

        public void Delete(string id)
        {
            DomainHelper.Publish(new DeleteUserEvent() { Id = id });
        }
    }
}
