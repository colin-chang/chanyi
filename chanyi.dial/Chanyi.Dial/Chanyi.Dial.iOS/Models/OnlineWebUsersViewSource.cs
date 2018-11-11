using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using Chanyi.Dial.Utils;

namespace Chanyi.Dial.iOS
{
    public class OnlineWebUsersViewSource : UITableViewSource
    {
        public IList<string> OnlineWebUsers { get; set; }
        string CellIdentifier = "TableCell";

        public OnlineWebUsersViewSource(List<string> onlineUsrs)
        {
            this.OnlineWebUsers = onlineUsrs;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return OnlineWebUsers.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);

            //显示文本
            string item = OnlineWebUsers[indexPath.Row];
            cell.TextLabel.Text = item;

            //设置选中状态
            if (Storage.BindUsers.Contains(item))
                cell.Accessory = UITableViewCellAccessory.Checkmark;
            else
                cell.Accessory = UITableViewCellAccessory.None;

            //样式
            cell.TextLabel.TextColor = UIColor.White;
            cell.BackgroundColor = UIColor.FromRGB(36, 126, 165);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            return cell;
        }

        //重写选中行效果
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var current = tableView.CellAt(indexPath).Accessory;
            if (current == UITableViewCellAccessory.None)
            {
                tableView.CellAt(indexPath).Accessory = UITableViewCellAccessory.Checkmark;
                Storage.BindUsers.Add(OnlineWebUsers[indexPath.Row]);
            }
            else if (current == UITableViewCellAccessory.Checkmark)
            {
                tableView.CellAt(indexPath).Accessory = UITableViewCellAccessory.None;
                Storage.BindUsers.Remove(OnlineWebUsers[indexPath.Row]);
            }
        }
    }
}

