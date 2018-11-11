using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Threading;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Collections.ObjectModel;


namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class AddAbortionViewModel : AddViewModel
    {
        public AddAbortionViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }
        public AddAbortionViewModel(bool withinInitilization) { }
        protected override void InitializeBindItem()
        {
            var females = this.Service.GetAbortionSheepBind();
 
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Females.Clear();
                this.Females.Add(new SheepBind { SerialNumber = defaultSelection });
                females.ForEach(f => this.Females.Add(f));

            }),DispatcherPriority.DataBind, null);

            if (this.isContinue) return;
            var principals = this.Service.GetEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }

        private ObservableCollection<SheepBind> females = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Females { get { return females; } }
        

        private string femaleId;
        [EntityProperty]
        [Required(ErrorMessage = "流产母羊必填")]
        public string FemaleId
        {
            get { return femaleId; }
            set
            {
                femaleId = value;
                this.Validate("FemaleId");
            }
        }

        private string reason;
        [EntityProperty]
        [Required(ErrorMessage = "流产原因必填")]
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
        [EntityProperty]
        [Required(ErrorMessage = "处理方式必填")]
        public string Dispose
        {
            get { return dispose; }
            set
            {
                dispose = value;
                this.Validate("Dispose");
            }
        }

        private DateTime? abortionDate;
        [EntityProperty]
        [Required(ErrorMessage = "流产日期必填")]
        [BeforeToday(ErrorMessage = "流产日期需小于当前日期")]
        public DateTime? AbortionDate
        {
            get { return abortionDate; }
            set
            {
                abortionDate = value;
                this.Validate("AbortionDate");
            }
        }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "技术员必选")]
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
                    var result = this.Service.AddAbortion(this.FemaleId, this.Reason, this.Dispose, (DateTime)this.AbortionDate, this.PrincipalId, this.UserId, this.Remark);

                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("流产记录添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
