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
    class GrantPermission2UserViewModel : GrantViewModel<Permission>
    {
        public GrantPermission2UserViewModel(string userId, Action<bool> grantFailed)
        {
            this.ObjectId = userId;
            this.GrantFailed = this.GrantFailed;
            this.InitializeBindData();
        }

        public GrantPermission2UserViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            var availablePerms = this.Service.GetUserAvailablePermissions(this.Keyword, this.ObjectId);
            var ownPerms = this.Service.GetPermissionByUserId(this.ObjectId);
            this.UIDispatcher.Invoke(new Action(() =>
            {
                availablePerms.ForEach(p => this.SourceItems.Add(p));
                ownPerms.ForEach(p => this.TargetItems.Add(p));
            }));
        }


        public override string Title { get { return string.Format("{0}-分配权限", this.Service.GetUserById(this.ObjectId).UserName); } }

        protected override Func<string, string, List<Permission>> GetSourceItemsMethod()
        {
            return this.Service.GetUserAvailablePermissions;
        }

        protected override Func<List<string>, string, IServices.ServiceResult<bool>> GetGrantMethod()
        {
            return this.Service.GrantPermission2User;
        }
    }
}
