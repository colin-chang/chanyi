using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace Chanyi.Shepherd.WPF.ViewModels.DiseaseControl
{
    /// <summary>
    /// 编辑疾病治疗
    /// </summary>
    class EditTreatmentViewModel : EditViewModel
    {
         public EditTreatmentViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            return this.Service.GetTreatmentById(this.Id);
        }
        protected override void InitializeBindItem()
        {
            var sheeps = this.Service.GetSheepBind(new SheepBindFilter { Status = SheepStatusEnum.Nomal });
            var principals = this.Service.GetEmployeeBind();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Sheeps.Clear();
                sheeps.ForEach(s => this.Sheeps.Add(s));

                this.Principals.Clear();
                principals.ForEach(p => this.Principals.Add(p));

            }), DispatcherPriority.DataBind, null);
        }
        public string Id { get; set; }
        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }



        private string sheepId;
        
        [Required(ErrorMessage = "羊编号必选")]
        public string SheepId
        {
            get { return sheepId; }
            set
            {
                sheepId = value;
                this.Validate("SheepId");
            }
        }

        private DateTime startDate;
        
        [Required(ErrorMessage = "症状开始日期必填")]
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                this.Validate("StartDate");
            }
        }

        private string symptom;
        
        [Required(ErrorMessage = "症状必填")]
        public string Symptom
        {
            get { return symptom; }
            set
            {
                symptom = value;
                this.Validate("Symptom");
            }
        }

        private string disease;
        
        [Required(ErrorMessage = "疾病（诊断）必填")]
        public string Disease
        {
            get { return disease; }
            set
            {
                disease = value;
                this.Validate("Disease");
            }
        }

        private string treatmentPlan;
        
        [Required(ErrorMessage = "用药详情必填")]
        public string TreatmentPlan
        {
            get { return treatmentPlan; }
            set
            {
                treatmentPlan = value;
                this.Validate("TreatmentPlan");
            }
        }

        private string effect;
        
        [Required(ErrorMessage = "治疗结果必填")]
        public string Effect
        {
            get { return effect; }
            set
            {
                effect = value;
                this.Validate("Effect");
            }
        }

        private int treatmentDays;
        
        [Required(ErrorMessage = "治疗时长必填")]
        public int TreatmentDays
        {
            get { return treatmentDays; }
            set
            {
                treatmentDays = value;
                this.Validate("TreatmentDays");
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
                    var result = this.Service.UpdateTreatment(this.Symptom,this.StartDate,this.Disease,this.TreatmentPlan,this.TreatmentDays,this.Effect, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

    }
}
