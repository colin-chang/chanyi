using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

using Chanyi.Shepherd.IServices;
using Chanyi.Shepherd.QueryModel.SetttingsModel;
using Chanyi.Shepherd.WPF.Properties;

namespace Chanyi.Shepherd.WPF.ViewModels
{
    class AccountViewModel : FormViewModel
    {
        public AccountViewModel(PasswordBox pwd)
        {
            this.PasswordBox = pwd;
            if (!this.IsOSSupported)
                return;

            if (this.users.Count() > 0)
            {
                this.UserName = users.LastOrDefault().UserName;
                pwd.Password = users.LastOrDefault().Password;
                this.IsRemember = true;
            }

            //Settings.Default.SerialNumber = "";
            //Settings.Default.Save();
        }

        public PasswordBox PasswordBox { get; set; }

        /// <summary>
        /// 保存密码的用户集合
        /// </summary>
        List<SettingsUser> users = (Settings.Default.Users ?? new SettingsUsers(new List<SettingsUser>())).Users;

        public IEnumerable<string> UserNames
        {
            get { return users.Select(u => u.UserName).Reverse(); }
        }

        private string userName;

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                this.RaisePropertyChanged("UserName");
                var user = users.Where(u => u.UserName == value).FirstOrDefault();
                if (user == null)
                    this.PasswordBox.Password = null;
                else
                    this.PasswordBox.Password = user.Password;
            }
        }

        public string Password { get { return this.PasswordBox.Password; } }


        private bool isRemember;
        public bool IsRemember
        {
            get
            {
                return isRemember;
            }
            set
            {
                isRemember = value;
                this.RaisePropertyChanged("IsRemember");
            }
        }

        public DelegateCommand Login
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (!Validate())
                        return;
                    var result = this.Service.Login(this.UserName, this.Password);
                    if (!ValidateFailedServiceResult<string>(result))
                        return;

                    this.UserId = result.Result;
                    //缓存用户权限
                    this.Permissions = this.Service.GetAllPermissionByUserId(this.UserId);
                    RememberPassword();
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

        bool Validate()
        {
            if (string.IsNullOrWhiteSpace(this.UserName))
                this.errors["UserName"] = "用户名必填";
            else
                this.errors.Remove("UserName");
            if (string.IsNullOrWhiteSpace(this.Password))
                this.errors["Password"] = "密码必填";
            else
                this.errors.Remove("Password");
            this.RaisePropertyChanged("Error");
            return errors.Count() <= 0;
        }

        public DelegateCommand Cancel
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.CurrentWindow.DialogResult = false;
                });
            }
        }

        void RememberPassword()
        {
            Action Remember = () =>
            {
                var us = users.Where(u => u.UserName == this.UserName);
                if (this.IsRemember)
                {
                    if (us.Count() > 0)
                    {
                        SettingsUser user = us.FirstOrDefault();
                        if (user.Password == this.Password)
                            return;

                        users.Remove(user);
                        users.Add(new SettingsUser(this.UserName, this.Password));
                    }
                    else
                        users.Add(new SettingsUser(this.UserName, this.Password));
                }
                else if (us.Count() > 0)
                    users.Remove(us.FirstOrDefault());
                SettingsUsers urs = new SettingsUsers(users);
                Settings.Default.Users = urs;
                Settings.Default.Save();
            };

            Remember.BeginInvoke(ar =>
            {
                Remember.EndInvoke(ar as IAsyncResult);
            }, Remember);
        }
    }
}
