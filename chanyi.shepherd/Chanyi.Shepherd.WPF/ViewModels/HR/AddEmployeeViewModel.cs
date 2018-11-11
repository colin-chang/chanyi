using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.HR;

namespace Chanyi.Shepherd.WPF.ViewModels.HR
{
    class AddEmployeeViewModel : AddViewModel
    {
        public AddEmployeeViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }
        public AddEmployeeViewModel(bool withinInitilization) { }
        protected override void InitializeBindItem()
        {
           
            var principals = this.Service.GetEmployeeBindWithDefault();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Clear();
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);

            if (this.isContinue) return;
            var dutys = this.Service.GetDutyBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Dutys.Add(new DutyBind { Name = defaultSelection });
                dutys.ForEach(d => this.Dutys.Add(d));
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

        private GenderEnum gender;
        [EntityProperty]
        public GenderEnum Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                this.RaisePropertyChanged("Gender");
            }
        }

        private string idNum;
        [EntityProperty]
        [IdNum(ErrorMessage = "身份证输入不合法")]
        public string IdNum
        {
            get { return idNum; }
            set
            {
                idNum = value;
                this.Validate("IdNum");
            }
        }

        private DateTime? entryDate;
        [EntityProperty]
        [Required(ErrorMessage = "入职时间必填")]
        public DateTime? EntryDate
        {
            get { return entryDate; }
            set
            {
                entryDate = value;
                this.Validate("EntryDate");
            }
        }

        private string salary;
        [EntityProperty]
        [Required(ErrorMessage = "员工工资必填")]
        [FloatNumber(0, int.MaxValue, ErrorMessage = "工资输入不合法")]
        public string Salary
        {
            get { return salary; }
            set
            {
                salary = value;
                this.Validate("Salary");
            }
        }

        private string serialNum;
        [EntityProperty]
        [Required(ErrorMessage = "工号必填")]
        public string SerialNum
        {
            get { return serialNum; }
            set
            {
                serialNum = value;
                this.Validate("SerialNum");
            }
        }

        private ObservableCollection<DutyBind> dutys = new ObservableCollection<DutyBind>();
        public ObservableCollection<DutyBind> Dutys { get { return dutys; } }

        private string dutyId;

        [EntityProperty]
        [Required(ErrorMessage = "职务必填")]
        public string DutyId
        {
            get { return dutyId; }
            set
            {
                dutyId = value;
                this.Validate("DutyId");
            }
        }

        private EmployeeStatusEnum status;
        [EntityProperty]
        public EmployeeStatusEnum Status
        {
            get { return status; }
            set
            {
                status = value;
                this.RaisePropertyChanged("Status");
            }
        }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }
        private string principalId;
        [EntityProperty]
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
                    var result = this.Service.AddEmployee(this.Name, this.Gender, this.IdNum, (DateTime)this.EntryDate, decimal.Parse(this.Salary), this.SerialNum, this.DutyId, this.Status, this.PrincipalId, this.UserId, this.Remark);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("员工只添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

        public DelegateCommand AddDuty
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddDutyWindow win = new AddDutyWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        var dutys = this.Service.GetDutyBind();
                        this.UIDispatcher.Invoke(new Action(() =>
                        {
                            this.Dutys.Clear();
                            this.Dutys.Add(new DutyBind { Name = defaultSelection });
                            dutys.ForEach(d => this.Dutys.Add(d));
                            this.DutyId = this.Dutys.FirstOrDefault() == null ? null : this.Dutys.FirstOrDefault().Id;
                        }), null);
                    }
                });
            }
        }
    }
}
