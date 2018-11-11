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
    class GrantRole2UserViewModel : GrantViewModel<Role>
    {
        public GrantRole2UserViewModel(string userId, Action<bool> grantFailed)
        {
            this.ObjectId = userId;
            this.GrantFailed = this.GrantFailed;
            this.InitializeBindData();
        }

        public GrantRole2UserViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            var availableRoles = this.Service.GetUserAvailableRoles(this.Keyword, this.ObjectId);
            var ownRoles = this.Service.GetAllRoleByUserId(this.ObjectId);
            this.UIDispatcher.Invoke(new Action(() =>
            {
                availableRoles.ForEach(r => this.SourceItems.Add(r));
                ownRoles.ForEach(r => this.TargetItems.Add(r));
            }));
        }

        public override string Title { get { return string.Format("{0}-设置角色", this.Service.GetUserById(this.ObjectId).UserName); } }



        protected override Func<string, string, List<Role>> GetSourceItemsMethod()
        {
            return this.Service.GetUserAvailableRoles;
        }

        protected override Func<List<string>, string, IServices.ServiceResult<bool>> GetGrantMethod()
        {
            return this.Service.GrantRole2User;
        }
    }
}
