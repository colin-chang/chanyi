using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.WPF.Expands.ValidateRule;


namespace Chanyi.Shepherd.WPF.ViewModels.SystemService
{
    public class AddRoleViewModel : AddViewModel
    {
        public AddRoleViewModel( UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddRoleViewModel(bool withinInitilization)
        {
        }


        private string name;
        [EntityProperty]
        [Required(ErrorMessage = "角色名称必填")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.Validate("Name");
            }
        }

        private string description;
        [EntityProperty]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                this.RaisePropertyChanged("Description");
            }
        }
        

        private string remark;
        [EntityProperty]
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                this.RaisePropertyChanged("Remark");
            }
        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                    {
                        var result = this.Service.AddRole(this.Name, this.Description, this.UserId, this.Remark);

                        this.errorControl = err;
                        if (!ValidateFailedServiceResult<string>(result))
                            return;
                        if (this.Continue2Add("角色创建成功"))
                            return;

                        this.CurrentWindow.DialogResult = true;
                    });
            }
        }
    }
}
