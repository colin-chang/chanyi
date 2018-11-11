using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Threading;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;


namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class AddAblactationViewModel : AddViewModel
    {
        public AddAblactationViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }
        public AddAblactationViewModel(bool withinInitilization){}
        protected override void InitializeBindItem()
        {
            var sheeps = this.Service.GetSheepBind(new QueryModel.BindingFilter.SheepBindFilter { GrowthStage = GrowthStageEnum.Lamb, Status = SheepStatusEnum.Nomal });
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Sheeps.Clear();
                this.Sheeps.Add(new SheepBind { SerialNumber = defaultSelection });
                sheeps.ForEach(s => this.Sheeps.Add(s));
            }),DispatcherPriority.DataBind,null);

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
        [Required(ErrorMessage = "羔羊编号输入错误")]
        public string SheepId
        {
            get { return sheepId; }
            set
            {
                sheepId = value;
                this.Validate("SheepId");
            }
        }

        private string ablactationWeight;

        [Required(ErrorMessage = "断奶羊重必填")]
        [EntityProperty]
        [FloatNumber(0, 30, ErrorMessage = "体重输入不合法")]
        public string AblactationWeight
        {
            get { return ablactationWeight; }
            set
            {
                ablactationWeight = value;
                this.Validate("AblactationWeight");
            }
        }

        private DateTime? ablactationDate;
        [EntityProperty]
        [Required(ErrorMessage = "断奶日期必填")]
        [BeforeToday(ErrorMessage = "断奶日期需小于当前日期")]
        public DateTime? AblactationDate
        {
            get { return ablactationDate; }
            set
            {
                ablactationDate = value;
                this.Validate("AblactationDate");
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
                    var result = this.Service.AddAblactation(this.SheepId, float.Parse(this.AblactationWeight), (DateTime)this.AblactationDate, this.PrincipalId, this.UserId, this.Remark);

                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    this.UpdateNotification();
                    if (this.Continue2Add("断奶记录添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
