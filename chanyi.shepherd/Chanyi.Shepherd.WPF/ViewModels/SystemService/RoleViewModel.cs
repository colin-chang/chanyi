using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.Model.System;
using Chanyi.Shepherd.WPF.Views.SystemService;
using System.Windows;

namespace Chanyi.Shepherd.WPF.ViewModels.SystemService
{
    class RoleViewModel : ListViewModel
    {
        public RoleViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
        }

        protected override void InitializeBindData()
        {
        }

        #region 数据列表
        public IEnumerable<Role> RoleList { get; set; }
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
                this.RoleList = this.Service.GetAllRoles(null, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<Role>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.RoleList);
            };
            this.ProgressRing.Show();
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);
        }
        #endregion

        //创建角色
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddRoleWindow win = new AddRoleWindow { Owner = CurrentWindow };
                    if (win.ShowDialog() == true)
                        this.LoadData();
                });
            }
        }

        //编辑角色
        public DelegateCommand<string> EditCommand
        {
            get
            {
                return this.GetEditCommand(id =>
                {
                    EditRoleWindow win = new EditRoleWindow(id);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        this.LoadData();
                });
            }
        }

        //分配权限
        public DelegateCommand<string> GrantPermissionCommand
        {
            get
            {
                return this.GetGrantCommand(id =>
                {
                    GrantPermission2RoleWindow win = new GrantPermission2RoleWindow(id);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        MessageBox.Show(this.CurrentWindow, "权限分配成功，用户重新登录后生效！","提示",MessageBoxButton.OK,MessageBoxImage.Information);
                        return;
                    }
                    if (win.GrantFailed)
                        MessageBox.Show(this.CurrentWindow, "分配权限出错，请稍后重试或联系开发人员！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                });
            }
        }
    }
}
