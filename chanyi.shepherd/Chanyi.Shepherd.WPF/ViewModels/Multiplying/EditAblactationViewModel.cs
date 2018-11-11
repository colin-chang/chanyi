using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class EditAblactationViewModel : EditViewModel
    {

        public EditAblactationViewModel(string editItemId)
        {
            this.Id = editItemId;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            return this.Service.GetAblactationById(this.Id);
        }
        protected override void InitializeBindItem()
        {
            this.Principals = this.Service.GetAllEmployeeBind();
        }
        public string Id { get; set; }

        public string SheepId { get; set; }

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
        private string ablactationWeight;

        [Required(ErrorMessage = "断奶羊重必填")]
        [FloatNumber(0, 100, ErrorMessage = "重量输入不合法")]
        public string AblactationWeight
        {
            get { return ablactationWeight; }
            set
            {
                ablactationWeight = value;
                this.Validate("AblactationWeight");
            }
        }

        private DateTime ablactationDate;

        [BeforeToday(ErrorMessage = "断奶日期需小于当前日期")]
        public DateTime AblactationDate
        {
            get { return ablactationDate; }
            set
            {
                ablactationDate = value;
                this.Validate("AblactationDate");
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
                    var result = this.Service.UpdateAblactation(float.Parse(this.AblactationWeight), this.AblactationDate, this.SheepId, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
