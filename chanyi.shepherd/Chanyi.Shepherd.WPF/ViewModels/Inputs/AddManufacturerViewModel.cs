using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class AddManufacturerViewModel : AddViewModel
    {
        public AddManufacturerViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddManufacturerViewModel(bool withinInitilization) { }
        protected override void InitializeBindItem()
        {
            if (this.isContinue)
                return;

            this.Principals = this.Service.GetEmployeeBind();
            this.Principals.Insert(0, new EmployeeBind { Name = defaultSelection });
        }

        private string name;
        [EntityProperty]
        [Required(ErrorMessage = "生产商必填")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.Validate("Name");
            }
        }

        private string department;
        [EntityProperty]
        public string Department
        {
            get { return department; }
            set
            {
                department = value;
                this.RaisePropertyChanged("Department");
            }
        }

        private string contactInfo;
        [EntityProperty]
        public string ContactInfo
        {
            get { return contactInfo; }
            set
            {
                contactInfo = value;
                this.RaisePropertyChanged("ContactInfo");
            }
        }

        private List<EmployeeBind> principals;
        public List<EmployeeBind> Principals
        {
            get { return principals; }
            set
            {
                principals = value;
                this.RaisePropertyChanged("Principals");
            }
        }
        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "操作人必选")]
        public string PrincipalId
        {
            get { return principalId; }
            set
            {
                principalId = value;
                this.Validate("PrincipalId");
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
                    var result = this.Service.AddManufacturer(this.Name, this.Department, this.ContactInfo, this.UserId, this.PrincipalId, this.Remark);
                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("生产商添加成功"))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
