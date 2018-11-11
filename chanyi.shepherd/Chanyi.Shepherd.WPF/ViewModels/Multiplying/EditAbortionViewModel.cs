using System;
using System.Collections.Generic;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;


namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class EditAbortionViewModel : EditViewModel
    {
        public EditAbortionViewModel(string editItemId)
        {
            this.Id = editItemId;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            return this.Service.GetAbortionById(this.Id);
        }
        public string Id { get; set; }
        protected override void InitializeBindItem()
        {
            this.Principals = this.Service.GetAllEmployeeBind();
        }


        private string femaleNumber;
        public string FemaleNumber
        {
            get { return femaleNumber; }
            set
            {
                femaleNumber = value;
                this.RaisePropertyChanged("FemaleNumber");
            }
        }

        private string reason;
        public string Reason
        {
            get { return reason; }
            set
            {
                reason = value;
                this.Validate("Reason");
            }
        }

        private string dispose;
        public string Dispose
        {
            get { return dispose; }
            set
            {
                dispose = value;
                this.Validate("Dispose");
            }
        }

        private DateTime abortionDate;

        [BeforeToday(ErrorMessage = "流产日期需小于当前日期")]
        public DateTime AbortionDate
        {
            get { return abortionDate; }
            set
            {
                abortionDate = value;
                this.Validate("AbortionDate");
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
                    var result = this.Service.UpdateAbortion(this.Reason, this.Dispose, this.AbortionDate, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
