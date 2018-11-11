using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Chanyi.Dial.Utils;

namespace Chanyi.Dial.Droid
{
    [Activity (Label = "在线电脑端用户")]
    public class OnlineWebUsersActivity : ListActivity
    {
        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            //var webUsers = Intent.Extras.GetStringArrayList ("webUsers") ?? new string [0];
            //this.ListAdapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SelectDialogMultiChoice, webUsers);
            ListAdapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SelectDialogMultiChoice, Storage.OnlineWebUsers.ToArray ());
            ListView.ChoiceMode = ChoiceMode.Multiple;
        }

        protected override void OnResume ()
        {
            base.OnResume ();
            //展示现有绑定关系
            var olnusrs = Storage.OnlineWebUsers.ToArray ();
            for (int i = 0; i < olnusrs.Length; i++) {
                var usr = olnusrs [i];
                ListView.SetItemChecked (i, Storage.BindUsers.Contains (usr));
            }
        }

        public override void OnBackPressed ()
        {
            base.OnBackPressed ();

            //更新绑定用户
            var array = ListView.CheckedItemPositions;
            Storage.BindUsers.Clear ();
            for (int i = 0; i < array.Size (); i++) {
                if (!array.ValueAt (i))
                    continue;
                int index = array.KeyAt (i);
                Storage.BindUsers.Add (ListAdapter.GetItem (index).ToString ());
            }
            Storage.IsAssigned = true;
        }
    }
}

