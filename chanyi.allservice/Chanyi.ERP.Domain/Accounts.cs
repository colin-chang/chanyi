using Chanyi.Common.Domain;
using Chanyi.ERP.Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Doamin
{
    public class Accounts
    {
        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Account { get; private set; }

        public string Password { get; private set; }

        public string CreaterId { get; private set; }

        public DateTime CreateTime { get; private set; }


        public string Create(string name, string account, string password, string createrId)
        {
            string id = Guid.NewGuid().ToString();

            DomainHelper.Publish(new CreateAccountEvent { Id = id, Name = name, Account = account, Password = password, CreaterId = createrId, CreateTime = DateTime.Now });

            return id;
        }

        public void Delete(string id)
        {
            DomainHelper.Publish(new DeleteAccountEvent { Id = id });
        }

        public void Update(string name, string account, string password,string id)
        {
            DomainHelper.Publish(new UpdateAccountEvent{ Id=id, Name=name, Account=account,Password=password });
        }
    }
}
