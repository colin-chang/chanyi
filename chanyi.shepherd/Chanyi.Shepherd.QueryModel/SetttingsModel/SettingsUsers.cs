using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.SetttingsModel
{
    public class SettingsUsers
    {
        public SettingsUsers()
        {
        }

        public SettingsUsers(List<SettingsUser> users)
        {
            this.Users = users;
        }
        public List<SettingsUser> Users { get; set; }
    }


    public class SettingsUser
    {
        public SettingsUser()
        {
        }

        public SettingsUser(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
