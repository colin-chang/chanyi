using System;
using System.Collections.Generic;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class EditMatingViewModel : EditViewModel
    {
        public EditMatingViewModel(string editItemId)
        {
            this.Id = editItemId;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            return this.Service.GetMatingById(this.Id);
        }
        protected override void InitializeBindItem()
        {
            this.Principals = this.Service.GetAllEmployeeBind();
        }

        public string Id { get; set; }
      
        private string femaleSerialNumber;
        public string FemaleSerialNumber
        {
            get { return femaleSerialNumber; }
            set
            {
                femaleSerialNumber = value;
                this.RaisePropertyChanged("FemaleSerialNumber");
            }
        }

        private string maleSerialNumber;
        public string MaleSerialNumber
        {
            get { return maleSerialNumber; }
            set
            {
                maleSerialNumber = value;
                this.RaisePropertyChanged("MaleSerialNumber");
            }
        }

        private string maleId;
        public string MaleId
        {
            get { return maleId; }
            set
            {
                maleId = value;
                this.Validate("MaleId");
            }
        }

        private string femaleId;
        public string FemaleId
        {
            get { return femaleId; }
            set
            {
                femaleId = value;
                this.Validate("FemaleId");
            }
        }

        private DateTime matingDate;

        [BeforeToday(ErrorMessage = "配种日期需小于当前日期")]
        public DateTime MatingDate
        {
            get { return matingDate; }
            set
            {
                matingDate = value;
                this.Validate("MatingDate");
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
                    var result = this.Service.UpdateMating(this.FemaleId, this.MaleId, this.MatingDate, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
