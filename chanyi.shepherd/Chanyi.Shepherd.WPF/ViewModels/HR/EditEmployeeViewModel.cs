using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Linq;
using System.Configuration;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.HR
{
    class EditEmployeeViewModel : EditViewModel
    {
        public EditEmployeeViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            return this.Service.GetEmployeeById(this.Id);
        }
        public string Id { get; set; }

        protected override void InitializeBindItem()
        {
            List<EmployeeBind> list = this.Service.GetEmployeeBind();
            list.Insert(0, new EmployeeBind { Name = ConfigurationManager.AppSettings["formDefaultSelection"] });


            this.Principals = list;
            this.Dutys = this.Service.GetDutyBind();
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

        private GenderEnum gender;
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

        private DateTime entryDate;
        public DateTime EntryDate
        {
            get { return entryDate; }
            set
            {
                entryDate = value;
                this.Validate("EntryDate");
            }
        }

        private string salary;
        [FloatNumber(0, 10000, ErrorMessage = "工资输入不合法")]
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
        private List<DutyBind> dutys;
        public List<DutyBind> Dutys
        {
            get { return dutys; }
            set
            {
                dutys = value;
                this.RaisePropertyChanged("Dutys");
            }
        }
        private string dutyId;
        [EntityProperty]
        public string DutyId
        {
            get { return dutyId; }
            set
            {
                dutyId = value;
                this.RaisePropertyChanged("DutyId");
            }
        }

        private EmployeeStatusEnum status;
        public EmployeeStatusEnum Status
        {
            get { return status; }
            set
            {
                status = value;
                this.RaisePropertyChanged("Status");
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

                       var result = this.Service.UpdateEmployee(this.Name, this.Gender, this.IdNum, (DateTime)this.EntryDate, decimal.Parse(this.Salary), this.SerialNum, this.DutyId, this.Status, this.PrincipalId, this.Remark, this.Id);
                        if (!ValidateFailedServiceResult<bool>(result))
                            return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
