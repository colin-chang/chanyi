using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.WPF.Expands.ValidateRule;


namespace Chanyi.Shepherd.WPF.ViewModels.SystemService
{
    public class AddUserViewModel : AddViewModel
    {
        public AddUserViewModel(PasswordBox pwd, UIElement error)
        {
            this.pwdBox = pwd;
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddUserViewModel(bool withinInitilization)
        {
        }

        private PasswordBox pwdBox;

        private string userName;
        [EntityProperty]
        [Required(ErrorMessage = "用户名必填")]
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                this.Validate("UserName");
            }
        }


        [EntityProperty]
        [Required(ErrorMessage = "密码必填")]
        public string Password
        {
            get { return this.pwdBox == null ? null : this.pwdBox.Password; }
            set
            {
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    if (this.pwdBox == null)
                        return;
                    this.pwdBox.Password = value;
                    this.Validate("Password");
                }));
            }
        }

        private string remark;
        [EntityProperty]
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                this.RaisePropertyChanged("Remark");
            }
        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(() => this.SetError("Password", null), err =>
                    {
                        var result = this.Service.AddUser(this.UserName, this.Password, this.UserId, this.Remark);

                        this.errorControl = err;
                        if (!ValidateFailedServiceResult<string>(result))
                            return;
                        if (this.Continue2Add("用户添加成功"))
                            return;

                        this.CurrentWindow.DialogResult = true;
                    });
            }
        }
    }
}
