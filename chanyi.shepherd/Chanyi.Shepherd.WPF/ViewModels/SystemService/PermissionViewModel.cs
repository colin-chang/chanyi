using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.Model.System;
using Chanyi.Shepherd.WPF.Views.SystemService;

namespace Chanyi.Shepherd.WPF.ViewModels.SystemService
{
    class PermissionViewModel : ListViewModel
    {
        public PermissionViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;
        }

        protected override void InitializeBindData()
        {
        }

        #region 数据列表
        public IEnumerable<Permission> PermissionList { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action initialize = () =>
            {
                int count;
                this.PermissionList = this.Service.GetAllPermissions(null,this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<Permission>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.PermissionList);
            };
            this.ProgressRing.Show();
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);
        }
        #endregion

        //编辑权限
        public DelegateCommand<string> EditCommand
        {
            get
            {
                return this.GetEditCommand(id =>
                {
                    EditPermissionWindow win = new EditPermissionWindow(id);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        this.LoadData();
                });
            }
        }
    }
}
