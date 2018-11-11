using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Threading;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.GroupManage
{
    class AddDeathManageViewModel : AddViewModel
    {
        public AddDeathManageViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddDeathManageViewModel(bool withinInitilization) { }
        protected override void InitializeBindItem()
        {         
            var sheeps = this.Service.GetSheepBind(new QueryModel.BindingFilter.SheepBindFilter { Status = SheepStatusEnum.Nomal});
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Sheeps.Clear();
                this.Sheeps.Add(new SheepBind() { SerialNumber = defaultSelection });
                sheeps.ForEach(s => this.Sheeps.Add(s));
            }),DispatcherPriority.DataBind, null);

            if (this.isContinue) return;
            var principals = this.Service.GetEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }


        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }
        
        private string sheepId;
        [EntityProperty]
        [Required(ErrorMessage = "死亡羊必选")]
        public string SheepId
        {
            get { return sheepId; }
            set
            {
                sheepId = value;
                this.Validate("SheepId");
            }
        }

        private string reason;
        [EntityProperty]
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

        private DateTime? deathDate;
        [EntityProperty]
        [Required(ErrorMessage = "死亡日期必填")]
        [BeforeToday(ErrorMessage = "死亡日期需小于当前日期")]
        public DateTime? DeathDate
        {
            get { return deathDate; }
            set
            {
                deathDate = value;
                this.Validate("DeathDate");
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
                    var result = this.Service.AddDeathManage(this.SheepId, this.Reason, this.Dispose, (DateTime)this.DeathDate, this.PrincipalId, this.UserId, this.Remark);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    this.UpdateNotification();
                    if (this.Continue2Add("死亡记录添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

    }
}
