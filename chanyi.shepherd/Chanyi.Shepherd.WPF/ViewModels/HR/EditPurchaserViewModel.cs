using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;



namespace Chanyi.Shepherd.WPF.ViewModels.HR
{
    class EditPurchaserViewModel : EditViewModel
    {
        public EditPurchaserViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            //return this.Service.GetPurchaser(new PurchaserFilter { Id = this.Id }).FirstOrDefault();
            return this.Service.GetCooperaterById(this.Id);
        }
        protected override void InitializeBindItem()
        {
            this.Principals = this.Service.GetEmployeeBind();
        }
        public string Id { get; set; }
        private string name;

        [Required(ErrorMessage = "姓名必填")]
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
        public string PrincipalId
        {
            get { return principalId; }
            set
            {
                principalId = value;
                this.RaisePropertyChanged("PrincipalId");
            }
        }
        private string remark;
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
                    var result = this.Service.UpdatePurchaser(this.Name, this.Department, this.ContactInfo, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
