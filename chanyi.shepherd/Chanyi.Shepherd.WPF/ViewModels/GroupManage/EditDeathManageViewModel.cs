using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.GroupManage
{
    class EditDeathManageViewModel : EditViewModel
    {
        public EditDeathManageViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            return this.Service.GetDeathManageById(this.Id);
        }
        protected override void InitializeBindItem()
        {
            this.Principals = this.Service.GetEmployeeBind();
        }
        public string Id { get; set; }

        private string serialNumber;
        public string SerialNumber
        {
            get { return serialNumber; }
            set
            {
                serialNumber = value;
                this.RaisePropertyChanged("SerialNumber");
            }
        }

        private string reason;

        [Required(ErrorMessage = "死亡原因必填")]
        public string Reason
        {
            get { return reason; }
            set
            {
                reason = value;
                this.Validate("Reason");
            }
        }

        private DeathDisposeEnum dispose;
        public DeathDisposeEnum Dispose
        {
            get { return dispose; }
            set
            {
                dispose = value;
                this.RaisePropertyChanged("Dispose");
            }
        }

        private DateTime deathDate;

        [BeforeToday(ErrorMessage = "死亡日期大于当前日期")]
        public DateTime DeathDate
        {
            get { return deathDate; }
            set
            {
                deathDate = value;
                this.Validate("DeathDate");
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
                this.Validate("PrincipalId");
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
                    var result = this.Service.UpdateDeathManage(this.Reason, this.Dispose, this.DeathDate, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

    }
}
