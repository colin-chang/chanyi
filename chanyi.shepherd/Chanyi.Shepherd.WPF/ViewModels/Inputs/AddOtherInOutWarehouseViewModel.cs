using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Inputs;


namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class AddOtherInOutWarehouseViewModel : AddViewModel
    {
        public AddOtherInOutWarehouseViewModel(UIElement error, InOutWarehouseDirectionEnum direction)
        {
            this.errorControl = error;
            this.InitializeBindData();
            this.direction = direction;
        }
        public AddOtherInOutWarehouseViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            if (this.isContinue)
                return;

            var names = this.Service.GetOtherBind();
            var principals = this.Service.GetEmployeeBind();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Names.Add(new OtherBind { Name = defaultSelection });
                names.ForEach(n => this.Names.Add(n));
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }

        InOutWarehouseDirectionEnum direction;

        public string Title { get { return this.direction == InOutWarehouseDirectionEnum.In ? "物品入库" : "物品出库"; } }


        private ObservableCollection<OtherBind> names = new ObservableCollection<OtherBind>();
        public ObservableCollection<OtherBind> Names { get { return names; } }


        private string id;
        [EntityProperty]
        [Required(ErrorMessage = "物品名称必填")]
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                this.RaisePropertyChanged("Id");
            }
        }

        private string amount;
        [EntityProperty]
        [Required(ErrorMessage = "数量必填")]
        [FloatNumber(0, 1000000, ErrorMessage = "数量输入不合法")]
        public string Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                this.Validate("Amount");
            }
        }

        private string unit;
        [EntityProperty]
        [Required(ErrorMessage = "计量单位必填")]
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                this.RaisePropertyChanged("Unit");
            }
        }
        private OutWarehouseDispositonEnum dispositon;
        [EntityProperty]
        [Required(ErrorMessage = "物品去向必选")]
        public OutWarehouseDispositonEnum Dispositon
        {
            get { return dispositon; }
            set
            {
                dispositon = value;
                this.Validate("Dispositon");
            }
        }

        private DateTime? operationDate;
        [EntityProperty]
        [BeforeToday(ErrorMessage = "日期需小于当前日期")]
        [Required(ErrorMessage = "日期必填")]
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
        [Required(ErrorMessage = "操作人必选")]
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

        public DelegateCommand SelectOtherNameChanged
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.Unit = this.Id == null ? null : this.Names.Where(a => a.Id == this.Id).FirstOrDefault().Unit;
                });
            }
        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.AddOtherInOutWarehouse(this.id, float.Parse(this.Amount), this.Unit, this.direction, (DateTime)this.OperationDate, this.PrincipalId, this.UserId, this.Remark,this.Dispositon);


                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    string str = this.direction == InOutWarehouseDirectionEnum.In ? "物品入库成功" : "物品出库成功";
                    if (this.Continue2Add(str))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

        /// <summary>
        /// 添加其他物品
        /// </summary>
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddOtherWindow win = new AddOtherWindow();
                    win.Owner = CurrentWindow;
                    win.ShowDialog();
                });
            }
        }
    }
}

