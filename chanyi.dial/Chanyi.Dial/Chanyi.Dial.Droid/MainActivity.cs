using Android.App;
using Android.Widget;
using Android.OS;
using Android.Telephony;
using Android.Content;
using Android.Net;

using System.IO;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using System;
using System.Resources;
using Chanyi.Dial.Utils;


namespace Chanyi.Dial.Droid
{
    [Activity (Label = "58赶集二手房拨号助手", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private string UserId {
            get {
#if DEBUG
                return "17090099602";
#endif
                return ((TelephonyManager)GetSystemService (TelephonyService)).Line1Number;
            }
        }

        #region 控件
        private Button btnStart;
        private Button btnBind;
        private EditText txtIP;
        private EditText txtPort;
        #endregion

        private IDial dial;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            SetContentView (Resource.Layout.Main);

            InitializeComponent ();
            this.dial = new DroidDial (txtIP.Text, txtPort.Text, UserId, this);
        }

        protected async override void OnResume ()
        {
            base.OnResume ();

            if (Storage.IsAssigned && Storage.BindUsers != null)
                await dial.BindWebUser (Storage.BindUsers.ToArray (), EnableListening);
        }

        protected override void OnDestroy ()
        {
            base.OnDestroy ();

            dial.Destroy ();
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <returns>The component.</returns>
        private void InitializeComponent ()
        {
            btnStart = FindViewById<Button> (Resource.Id.btnStart);
            btnBind = FindViewById<Button> (Resource.Id.btnBind);
            txtIP = FindViewById<EditText> (Resource.Id.txtIP);
            txtPort = FindViewById<EditText> (Resource.Id.txtPort);

            btnStart.Click += async (sender, e) => {
                btnStart.Enabled = false;
                UpdateUrl ();
                await dial.StartSession (success => {
                    if (!success) {
                        dial.ShowDialog ("开启监听失败，请确保通信服务已开启并保持网络畅通！");
                        EnableListening ();
                    } else {
                        btnStart.Text = "拨号监听已开启";
                        btnBind.Enabled = true;
                    }
                });
            };

            btnBind.Click += (sender, e) => dial.ReBindWebUser (() => StartActivity (new Intent (this, typeof (OnlineWebUsersActivity))), EnableListening);
        }

        private void UpdateUrl ()
        {
            this.dial.Host = txtIP.Text;
            this.dial.Port = txtPort.Text;
        }

        private void EnableListening ()
        {
            btnStart.Text = "开启拨号监听";
            btnStart.Enabled = true;
            btnBind.Enabled = false;
        }
    }
}