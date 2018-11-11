using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.Model.HR;
using Chanyi.Shepherd.WPF.Views.SystemService;

namespace Chanyi.Shepherd.WPF.ViewModels.SystemService
{
    class UserViewModel : ListViewModel
    {
        public UserViewModel(string header, string icon, string intro, string _editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = _editPermUrl;
        }

        protected override void InitializeBindData()
        {   
        }

        #region 数据列表
        public IEnumerable<User> UserList { get; set; }
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
                this.UserList = this.Service.GetUser(this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<User>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide(); 
                }), this.UserList);
            };
            this.ProgressRing.Show();
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);
        }
        #endregion

        //重置密码
        public DelegateCommand<string> ResetPasswordCommand
        {
            get
            {
                return new DelegateCommand<string>(d =>
                {
                    ResetUserPasswordWindow win = new ResetUserPasswordWindow(d);
                    win.Owner = CurrentWindow;
                    win.ShowDialog();
                });
            }
        }

        //分配权限
        public DelegateCommand<string> GrantPermissionCommand
        {
            get
            {
                return new DelegateCommand<string>(userId =>
                {
                    GrantPermission2UserWindow win = new GrantPermission2UserWindow(userId);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        MessageBox.Show(this.CurrentWindow, "权限分配成功，用户重新登录后生效！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    if (win.GrantFailed)
                        MessageBox.Show(this.CurrentWindow, "分配权限出错，请稍后重试或联系开发人员！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                });
            }
        }

        //设置角色
        public DelegateCommand<string> GrantRoleCommand
        {
            get
            {
                return new DelegateCommand<string>(userId =>
                {
                    GrantRole2UserWindow win = new GrantRole2UserWindow(userId);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        MessageBox.Show(this.CurrentWindow, "角色设置成功，用户重新登录后生效！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    if (win.GrantFailed)
                        MessageBox.Show(this.CurrentWindow, "设置角色出错，请稍后重试或联系开发人员！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                });
            }
        }


        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetUser(rowCount).ToArray();
        }
    }
}