using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Collections.ObjectModel;

namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class AddBuySheepViewModel : AddViewModel
    {
        public AddBuySheepViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddBuySheepViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            var sheeps = this.Service.GetBuySheepBind4Add();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Sheeps.Clear();
                this.Sheeps.Add(new SheepBind { SerialNumber = defaultSelection });
                sheeps.ForEach(s => this.Sheeps.Add(s));
            }), DispatcherPriority.DataBind, null);

            if (this.isContinue) return;
            var principals = this.Service.GetEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = this.defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }

        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }


        private string sheepId;
        [EntityProperty]
        [Required(ErrorMessage = "请选中羊编号")]
        public string SheepId
        {
            get { return sheepId; }
            set
            {
                sheepId = value;
                this.Validate("SheepId");
            }
        }

        private string money;
        [EntityProperty]
        [Required(ErrorMessage = "购买价格必填")]
        [FloatNumber(0, int.MaxValue, ErrorMessage = "购买价格输入不合法")]
        public string Money
        {
            get { return money; }
            set
            {
                money = value;
                this.Validate("Money");
            }
        }

        private string weight;
        [EntityProperty]
        [FloatNumber(0, int.MaxValue, ErrorMessage = "羊重量输入不合法")]
        public string Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                this.RaisePropertyChanged("Weight");
            }
        }


        private string source;
        [EntityProperty]
        [Required(ErrorMessage = "羊只购买来源必填")]
        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                this.Validate("Source");
            }
        }


        private DateTime? operationDate;
        [EntityProperty]
        [Required(ErrorMessage = "购买日期必填")]
        [BeforeToday(ErrorMessage = "购买日期需小于当前日期")]
        public DateTime? OperationDate
        {
            get { return operationDate; }
            set
            {
                operationDate = value;
                this.Validate("OperationDate");
            }
        }
        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "购买人必填")]
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
                    var result = this.Service.AddBuySheep(this.SheepId, this.Source, decimal.Parse(this.Money), (DateTime)this.OperationDate, this.PrincipalId, this.UserId, this.Remark, float.Parse(this.Weight));

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("购买羊只添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}

