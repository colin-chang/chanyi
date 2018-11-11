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
    /// 编辑防疫计划
    /// </summary>
    class EditAntiepidemicPlanViewModel : EditViewModel
    {
        public EditAntiepidemicPlanViewModel(string id)
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
        
        [Required(ErrorMessage = "计划名称必填")]
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

        private DateTime planExecuteDate;
        
        [Required(ErrorMessage = "计划防疫日期必填")]
        public DateTime PlanExecuteDate
        {
            get { return planExecuteDate; }
            set
            {
                planExecuteDate = value;
                this.Validate("PlanExecuteDate");
            }
        }

        private string sheepFlock;

        
        [Required(ErrorMessage = "计划防疫羊群必填")]
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
                    var result = this.Service.UpdateAntiepidemicPlan(this.Name,this.Vaccine,this.PlanExecuteDate,this.SheepFlock, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

    }
}
