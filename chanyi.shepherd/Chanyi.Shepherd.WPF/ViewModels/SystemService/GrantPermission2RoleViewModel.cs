using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Model;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.QueryModel.Model.System;
using Chanyi.Shepherd.WPF.ViewModels.SystemService;



namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    class GrantPermission2RoleViewModel : GrantViewModel<Permission>
    {
        public GrantPermission2RoleViewModel(string roleId, Action<bool> grantFailed)
        {
            this.ObjectId = roleId;
            this.GrantFailed = this.GrantFailed;
            this.InitializeBindData();
        }

        public GrantPermission2RoleViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            var availablePerms = this.Service.GetRoleAvailablePermissions(this.Keyword, this.ObjectId);
            var ownPerms = this.Service.GetPermissionsByRoleId(this.ObjectId);
            this.UIDispatcher.Invoke(new Action(() =>
            {
                availablePerms.ForEach(p => this.SourceItems.Add(p));
                ownPerms.ForEach(p => this.TargetItems.Add(p));
            }));
        }


        public override string Title { get { return string.Format("{0}-分配权限", this.Service.GetRoleById(this.ObjectId).Name); } }

        protected override Func<string, string, List<Permission>> GetSourceItemsMethod()
        {
            return this.Service.GetRoleAvailablePermissions;
        }

        protected override Func<List<string>, string, IServices.ServiceResult<bool>> GetGrantMethod()
        {
            return this.Service.GrantPermission2Role;
        }
    }
}
