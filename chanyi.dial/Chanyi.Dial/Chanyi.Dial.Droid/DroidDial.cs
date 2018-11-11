using System;
using System.Threading.Tasks;
using Chanyi.Dial.Utils;
using Android.Content.PM;
using Android.Content;
using Android.App;
using Java.Lang;

namespace Chanyi.Dial.Droid
{
    public class DroidDial : BaseDial
    {
        public DroidDial (string host, string port, string userId, Activity currentActivity)
        {
            Host = host;
            Port = port;
            UserId = userId;
            this.currentActivity = currentActivity;
        }

        public override string Host { get; set; }

        public override string Port { get; set; }

        public override string UserId { get; set; }

        private Activity currentActivity;

        public override void CallPhoneNumber (string phoneNumber)
        {
            var callIntent = new Intent (Intent.ActionCall);
            callIntent.SetData (Android.Net.Uri.Parse ("tel:" + phoneNumber));
            currentActivity.StartActivity (callIntent);
        }

        public override void ShowDialog (string message)
        {
            var callDialog = new AlertDialog.Builder (currentActivity);
            callDialog.SetMessage (message);
            callDialog.SetNegativeButton ("确定", delegate { });
            callDialog.Show ();
        }
    }
}

