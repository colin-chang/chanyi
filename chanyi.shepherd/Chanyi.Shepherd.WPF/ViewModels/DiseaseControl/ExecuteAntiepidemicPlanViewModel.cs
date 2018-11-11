using Chanyi.Shepherd.QueryModel.BindingModel;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;

namespace Chanyi.Shepherd.WPF.ViewModels.DiseaseControl
{
    class ExecuteAntiepidemicPlanViewModel:EditViewModel
    {
        public ExecuteAntiepidemicPlanViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }

        protected override object GetEditModel()
        {
            return this.Service.GetAntiepidemicPlanById(this.Id);
        }

        protected override void InitializeBindItem()
        {
            this.Principals = this.Service.GetEmployeeBind();
        }
        public string Id { get; set; }

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

        private string vaccine;
        public string Vaccine
        {
            get { return vaccine; }
            set
            {
                vaccine = value;
                this.RaisePropertyChanged("Vaccine");
            }
        }

        private string sheepFlock;
        public string SheepFlock
        {
            get { return sheepFlock; }
            set
            {
                sheepFlock = value;
                this.RaisePropertyChanged("SheepFlock");
            }
        }

        private DateTime? executeDate;

        [Required(ErrorMessage = "执行防疫日期必填")]
        public DateTime? ExecuteDate
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
                    var result = this.Service.ExecuteAntiepidemicPlan(this.Id, (DateTime)this.ExecuteDate, this.Effect, this.PrincipalId, this.UserId, this.Remark);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
