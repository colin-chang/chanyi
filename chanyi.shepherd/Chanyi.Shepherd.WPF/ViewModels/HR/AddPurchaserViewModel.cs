using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Collections.ObjectModel;


namespace Chanyi.Shepherd.WPF.ViewModels.HR
{
    class AddPurchaserViewModel : AddViewModel
    {
        public AddPurchaserViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddPurchaserViewModel(bool withinInitilization) { }
        protected override void InitializeBindItem()
        {
            if (isContinue)
                return;
            var principals = this.Service.GetEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }

        private string name;
        [EntityProperty]
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

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

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
                    var result = this.Service.AddPurchaser(this.Name, this.Department, this.ContactInfo, this.UserId, this.PrincipalId, this.Remark);
                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("客户添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
