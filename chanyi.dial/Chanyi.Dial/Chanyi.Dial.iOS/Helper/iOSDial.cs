using System;
using Chanyi.Dial.Utils;
using Foundation;
using UIKit;

namespace Chanyi.Dial.iOS
{
    public class iOSDial : BaseDial
    {
        public iOSDial(string host, string port, string userId)
        {
            Host = host;
            Port = port;
            UserId = userId;
        }

        public override string Host { get; set; }

        public override string Port { get; set; }

        public override string UserId { get; set; }


        public override void CallPhoneNumber(string phoneNumber)
        {
            try
            {
                var dispacher = Storage.MainThreadDispacher;
                if (dispacher == null)
                    return;
                dispacher(() =>
                {
                    var url = new NSUrl(string.Format("tel:{0}", phoneNumber));
                    if (UIApplication.SharedApplication.CanOpenUrl(url))
                        UIApplication.SharedApplication.OpenUrl(url);
                    else
                        ShowDialog("此手机不能打电话");
                });
            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message);
            }
        }

        public override void ShowDialog(string message)
        {
            UIAlertView alert = new UIAlertView() { Title = "提示", Message = message };
            alert.AddButton("确定");
            alert.Show();
        }
    }
}

