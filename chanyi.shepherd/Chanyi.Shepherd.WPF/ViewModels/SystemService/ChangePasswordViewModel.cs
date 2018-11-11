using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Commands;

namespace Chanyi.Shepherd.WPF.ViewModels.SystemService
{
    class ChangePasswordViewModel : AddViewModel
    {
        private string oldPassword;
        [Required(ErrorMessage = "原密码必填")]
        public string OldPassword
        {
            get { return oldPassword; }
            set
            {
                oldPassword = value;
                this.Validate("OldPassword");
            }
        }

        private string newpassword;
        public string NewPassword
        {
            get { return newpassword; }
            set
            {
                newpassword = value;
                if (this.NewPassword != this.Password)
                    this.errors["Error"] = "两次密码不相同";
                else
                    this.errors.Remove("Error");

                if (string.IsNullOrWhiteSpace(this.NewPassword))
                    this.errors["NewPassword"] = "新密码必填";
                else
                    this.errors.Remove("NewPassword");
                this.RaisePropertyChanged("Error");
                this.RaisePropertyChanged("NewPassword");
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                if (this.NewPassword != this.Password)
                    this.errors["Error"] = "两次密码不相同";
                else
                    this.errors.Remove("Error");

                if (string.IsNullOrWhiteSpace(this.NewPassword))
                    this.errors["Password"] = "新密码必填";
                else
                    this.errors.Remove("Password");
                this.RaisePropertyChanged("Error");
                this.RaisePropertyChanged("Password");

            }
        }   

        public DelegateCommand<PasswordBox> PasswordLostFocus
        {
            get
            {
                return new DelegateCommand<PasswordBox>((pwd) =>
                {
                    pwd.Tag = pwd.Password;
                });
            }
        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.UpdatePassword(this.OldPassword, this.NewPassword, this.UserId);
                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
