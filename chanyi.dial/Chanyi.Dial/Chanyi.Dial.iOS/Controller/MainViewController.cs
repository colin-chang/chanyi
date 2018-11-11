using System;
using System.Linq;
using UIKit;

using Chanyi.Dial.Utils;
namespace Chanyi.Dial.iOS
{
    public partial class MainViewController : UIViewController
    {
        public IDial Dial
        {
            get;
            private set;
        }
        protected MainViewController(IntPtr handle) : base(handle)
        {
            Storage.MainThreadDispacher = this.BeginInvokeOnMainThread;
            Dial = new iOSDial(null, null, UIDevice.CurrentDevice.IdentifierForVendor.AsString());
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            btnStart.TouchUpInside += async (sender, e) =>
            {
                btnStart.Enabled = false;
                UpdateUrl();
                await Dial.StartSession(success =>
                {
                    if (!success)
                    {
                        Dial.ShowDialog("开启监听失败，请确保通信服务已开启并保持网络畅通！");
                        EnableListening();
                    }
                    else
                    {
                        btnStart.SetTitle("拨号监听已开启", UIControlState.Disabled);
                        btnBind.Enabled = true;
                    }
                });
            };

            btnBind.TouchUpInside += async (sender, e) => await Dial.ReBindWebUser(null, EnableListening);
        }

        private void UpdateUrl()
        {
            Dial.Host = txtIP.Text;
            Dial.Port = txtPort.Text;
        }

        private void EnableListening()
        {
            btnStart.SetTitle("开启拨号监听", UIControlState.Normal);
            btnStart.Enabled = true;
            btnBind.Enabled = false;
        }


        public async override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (Storage.IsAssigned && Storage.BindUsers != null)
                await Dial.BindWebUser(Storage.BindUsers.ToArray(), EnableListening);
        }
    }
}