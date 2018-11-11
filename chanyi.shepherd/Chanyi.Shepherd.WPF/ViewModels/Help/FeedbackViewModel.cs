using System.ComponentModel.DataAnnotations;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Windows;
using Chanyi.Shepherd.WPF.Properties;

namespace Chanyi.Shepherd.WPF.ViewModels.Help
{
    class FeedbackViewModel : FormViewModel
    {
        private string title;

        [Required(ErrorMessage = "标题必填")]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                this.Validate("Title");
            }
        }

        private string email;
        [NullableEmail(ErrorMessage = "联系邮箱不合法")]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                this.Validate("Email");
            }
        }

        private string desc;

        [Required(ErrorMessage = "描述必填")]
        public string Desc
        {
            get { return desc; }
            set
            {
                desc = value;
                this.Validate("Desc");
            }
        }

        public DelegateCommand SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand(() =>
                {
                    if (!this.NetworkAvailable)
                        MessageBox.Show("网络连接不可用！");
                    var result = this.Service.SendEmail(this.Title,
                        string.Format("Email:{0}\r\nProductId:{1}\r\nSerialNumber:{2}\r\nDesc:{3}",this.Email,Resources.ProductId,Settings.Default.SerialNumber,this.Desc));
                    if (result.Result)
                    {
                        MessageBox.Show("反馈发送成功，感谢您的宝贵意见和对我们的支持！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.CurrentWindow.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("抱歉，反馈发送失败，请稍后重试！\r\n详细信息：" + result.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.CurrentWindow.DialogResult = false;
                    }
                });
            }
        }
    }
}
