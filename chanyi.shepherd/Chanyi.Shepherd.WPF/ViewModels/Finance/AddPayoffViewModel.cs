using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Collections.ObjectModel;

namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class AddPayoffViewModel : AddViewModel
    {
        public AddPayoffViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddPayoffViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            if (this.isContinue) return;
            var list = this.Service.GetEmployeeBind();
            list.Insert(0, new EmployeeBind { Name = defaultSelection });
            this.UIDispatcher.Invoke(new Action(() => {
                list.ForEach(l => this.Employees.Add(l));
                list.ForEach(l => this.Principals.Add(l));
            }), null);
        }

        private ObservableCollection<EmployeeBind> employees = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Employees { get { return employees; } }
        

        private string employeeId;
        [EntityProperty]
        [Required(ErrorMessage = "员工必填")]
        public string EmployeeId
        {
            get { return employeeId; }
            set
            {
                employeeId = value;
                this.Validate("EmployeeId");
            }
        }

        private string money;
        [EntityProperty]
        [Required(ErrorMessage = "员工工资必填")]
        [FloatNumber(0, int.MaxValue, ErrorMessage = "员工工资输入不合法")]
        public string Money
        {
            get { return money; }
            set
            {
                money = value;
                this.Validate("Money");


            }
        }

        private DateTime? operationDate;
        [EntityProperty]
        [Required(ErrorMessage = "日期必填")]
        public DateTime? OperationDate
        {
            get { return operationDate; }
            set
            {
                operationDate = value;
                this.Validate("OperationDate");
            }
        }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "操作人必填")]
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
                    var result = this.Service.AddPayoff(this.EmployeeId, decimal.Parse(this.Money), (DateTime)this.OperationDate, this.PrincipalId, this.UserId, this.Remark);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("员工工资添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
