using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.HR;


namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class AddSellManureViewModel : AddViewModel
    {
        public AddSellManureViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddSellManureViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            if (this.isContinue)
                return;

            var purchasers = this.Service.GetPurchaserBind();
            var principals = this.Service.GetEmployeeBind();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = this.defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
                this.Purchasers.Insert(0, new PurchaserBind { Name = defaultSelection });
                purchasers.ForEach(p => this.Purchasers.Add(p));
            }), null);
        }
        private string price;
        [EntityProperty]
        [Required(ErrorMessage = "出售价格必填")]
        [FloatNumber(0, int.MaxValue, ErrorMessage = "出售价格输入不合法")]
        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                this.Validate("Price");


            }
        }

        private ObservableCollection<PurchaserBind> purchasers = new ObservableCollection<PurchaserBind>();
        public ObservableCollection<PurchaserBind> Purchasers { get { return purchasers; } }

        private string purchaserId;
        [EntityProperty]
        [Required(ErrorMessage = "购买者内容必填")]
        public string PurchaserId
        {
            get { return purchaserId; }
            set
            {
                purchaserId = value;
                this.Validate("PurchaserId");
            }
        }
        private DateTime? operationDate;
        [EntityProperty]
        [Required(ErrorMessage = "出售日期必填")]
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
        [Required(ErrorMessage = "操作人必填")]
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
                    var result = this.Service.AddSellManure(decimal.Parse(this.Price), this.PurchaserId, (DateTime)this.OperationDate, this.PrincipalId, this.UserId, this.Remark);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("羊粪出售记录添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

        public DelegateCommand AddPurchaserCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddPurchaserWindow win = new AddPurchaserWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        var purchasers = this.Service.GetPurchaserBind();
                        this.UIDispatcher.Invoke(new Action(() =>
                        {
                            this.Purchasers.Clear();
                            this.Purchasers.Add(new PurchaserBind { Name = defaultSelection });
                            purchasers.ForEach(m => this.Purchasers.Add(m));
                            this.PurchaserId = this.Purchasers.FirstOrDefault() == null ? null : this.Purchasers.FirstOrDefault().Id;
                        }), null);
                    }
                });
            }
        }
    }
}
