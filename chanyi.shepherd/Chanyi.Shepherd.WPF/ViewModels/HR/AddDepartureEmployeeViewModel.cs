using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Model.HR;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;

namespace Chanyi.Shepherd.WPF.ViewModels.HR
{
    class AddDepartureEmployeeViewModel : EditViewModel
    {
        public AddDepartureEmployeeViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            Employee employee = new Employee();
            Employee e = this.Service.GetEmployeeById(this.Id);
            employee.Name = e.Name;
            employee.Id = e.Id;
            return employee;

        }
        public string Id { get; set; }

        protected override void InitializeBindItem()
        {
            this.Principals = this.Service.GetEmployeeBind();
            this.Principals.Insert(0, new EmployeeBind { Name = ConfigurationManager.AppSettings["formDefaultSelection"] });
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        private string reason;
        [Required(ErrorMessage = "离职原因必填")]
        public string Reason
        {
            get { return reason; }
            set
            {
                reason = value;
                this.RaisePropertyChanged("Reason");
            }
        }

        private DateTime? departureDate;
        [Required(ErrorMessage = "离职日期必填")]
        public DateTime? DepartureDate
        {
            get { return departureDate; }
            set
            {
                departureDate = value;
                this.Validate("DepartureDate");
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
        [Required(ErrorMessage = "操作人必填")]
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
                    var result = this.Service.AddQuit(this.Id, this.Reason,(DateTime)this.DepartureDate ,this.PrincipalId, this.UserId, this.Remark);

                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
