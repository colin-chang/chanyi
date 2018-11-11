using System;
using System.Linq;
using UIKit;

using Chanyi.Dial.Utils;
using System.Collections.Generic;
using Foundation;

namespace Chanyi.Dial.iOS
{
    public partial class OnlineWebUsersViewController : UITableViewController
    {
        protected OnlineWebUsersViewController(IntPtr handle) : base(handle)
        {
            Storage.IsAssigned = true;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            table.Source = new OnlineWebUsersViewSource(Storage.OnlineWebUsers.ToList());
        }
    }
}