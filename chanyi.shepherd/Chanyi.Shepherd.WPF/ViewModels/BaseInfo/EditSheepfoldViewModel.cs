using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;

namespace Chanyi.Shepherd.WPF.ViewModels.BaseInfo
{
    class EditSheepfoldViewModel : EditViewModel
    {
        public EditSheepfoldViewModel(string editItemId)
        {
            this.Id = editItemId;
            this.InitializeBindData();
        }

        protected override object GetEditModel()
        {
            return this.Service.GetSheepFoldById(this.Id);
        }

        protected override void InitializeBindItem()
        {
            this.Administrators = this.Service.GetAllEmployeeBind();
        }

        public string Id { get; set; }

        private string name;
        [EntityProperty]
        [Required(ErrorMessage = "羊圈舍必填")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.Validate("Name");
            }
        }
        private List<EmployeeBind> administrators = new List<EmployeeBind>();

        public List<EmployeeBind> Administrators
        {
            get { return administrators; }
            set
            {
                this.administrators = value;
                this.RaisePropertyChanged("Administrators");
            }
        }

        private string administrator;
        [EntityProperty]
        [Required(ErrorMessage = "羊圈管理员必填")]
        public string Administrator
        {
            get { return administrator; }
            set
            {
                administrator = value;
                this.Validate("Administrator");
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
                    var result = this.Service.UpdateSheepFold(this.Name, this.Administrator, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result)) return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

    }
}
