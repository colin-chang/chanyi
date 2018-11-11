using Chanyi.Shepherd.QueryModel.BindingModel;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;

namespace Chanyi.Shepherd.WPF.ViewModels.DiseaseControl
{
    /// <summary>
    /// 编辑疾病实施信息
    /// </summary>
    class EditAntiepidemicViewModel : EditViewModel
    {
        public EditAntiepidemicViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            return this.Service.GetAntiepidemicById(this.Id);
        }
        protected override void InitializeBindItem()
        {
            this.Principals = this.Service.GetEmployeeBind();
        }
        public string Id { get; set; }

        private string name;

        [Required(ErrorMessage = "防疫名称必填")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.Validate("Name");
            }
        }

        private string vaccine;

        [Required(ErrorMessage = "疫苗必填")]
        public string Vaccine
        {
            get { return vaccine; }
            set
            {
                vaccine = value;
                this.Validate("Vaccine");
            }
        }

        private DateTime executeDate;

        [Required(ErrorMessage = "防疫日期必填")]
        public DateTime ExecuteDate
        {
            get { return executeDate; }
            set
            {
                executeDate = value;
                this.Validate("ExecuteDate");
            }
        }

        private string effect;

        public string Effect
        {
            get { return effect; }
            set
            {
                effect = value;
                this.RaisePropertyChanged("Effect");
            }
        }

        private string sheepFlock;

        [Required(ErrorMessage = "防疫羊群必填")]
        public string SheepFlock
        {
            get { return sheepFlock; }
            set
            {
                sheepFlock = value;
                this.Validate("SheepFlock");
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
                    var result = this.Service.UpdateAntiepidemic(this.Name, this.Vaccine, this.ExecuteDate, this.Effect, this.SheepFlock, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

    }
}
