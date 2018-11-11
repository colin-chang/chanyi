using Chanyi.Common.Domain;
using Chanyi.ERP.Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Doamin
{
    public class Duty
    {
        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Desc { get; private set; }

        public string DepartmentId { get; private set; }

        public bool IsEnabled { get; private set; }

        public string CreaterId { get; private set; }

        public DateTime CreateTime { get; private set; }

        public string Create(string name, string desc, string departmentId, string createrId)
        {
            string id = Guid.NewGuid().ToString();
            DomainHelper.Publish(new CreateDutyEvent() { CreaterId = createrId, CreateTime = DateTime.Now, DepartmentId = departmentId, Desc = desc, Id = id, IsEnabled = true, Name = name });
            return id;
        }

        public void Update(string name, string desc, string departmentId, bool isEnabled, string id)
        {
            DomainHelper.Publish(new UpdateDutyEvent() { DepartmentId = departmentId, Desc = desc, Id = id, IsEnabled = true, Name = name });
        }

        public void Delete(string id)
        {
            DomainHelper.Publish(new DeleteDutyEvent() { Id = id });
        }
    }
}
