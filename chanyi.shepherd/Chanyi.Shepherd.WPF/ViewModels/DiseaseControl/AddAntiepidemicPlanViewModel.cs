using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
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
    /// 添加防疫计划
    /// </summary>
    public class AddAntiepidemicPlanViewModel : AddViewModel
    {
        public AddAntiepidemicPlanViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddAntiepidemicPlanViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            //this.UIDispatcher.Invoke(new Action(() =>
            //{

            //}), DispatcherPriority.DataBind, null);

            if (this.isContinue) return;
            var principals = this.Service.GetEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }

        private string name;
        [EntityProperty]
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
        [EntityProperty]
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

        private DateTime? planExecuteDate;
        [EntityProperty]
        [BeforeToday(ErrorMessage = "执行计划防疫日期小于当前日期")]
        [Required(ErrorMessage = "执行计划防疫日期必填")]
        public DateTime? PlanExecuteDate
        {
            get { return planExecuteDate; }
            set
            {
                planExecuteDate = value;
                this.Validate("PlanExecuteDate");
            }
        }

        private string sheepFlock;

        [EntityProperty]
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

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "制定人必选")]
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
                    var result = this.Service.AddAntiepidemicPlan(this.Name, this.Vaccine, (DateTime)this.PlanExecuteDate, this.SheepFlock, this.PrincipalId, this.UserId, this.Remark);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    this.UpdateNotification();
                    if (this.Continue2Add("防疫计划添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

    }
}
