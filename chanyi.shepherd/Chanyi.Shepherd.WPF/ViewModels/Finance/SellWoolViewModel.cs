using System;
using System.Collections.Generic;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.QueryModel.Model.Finance;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Finance;
using System.Collections.ObjectModel;



namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class SellWoolViewModel : ListViewModel
    {
        public SellWoolViewModel(bool withinInitilization) { }

        public SellWoolViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<SellWoolViewModel, SellWoolFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {

            Action Initialize = () =>
            {
                var purchasers = this.Service.GetPurchaserBind();

                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Principals.Clear();
                    this.Purchasers.Clear();

                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));

                    this.Purchasers.Add(new PurchaserBind { Name = defaultSelection });
                    purchasers.ForEach(p => this.Purchasers.Add(p));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public SellWoolFilter Filter
        {
            get
            {
                SellWoolFilter filter = Mapper.Map<SellWoolFilter>(this);
                return filter;
            }
        }

        #region 绑定搜索字段

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private ObservableCollection<PurchaserBind> purchasers = new ObservableCollection<PurchaserBind>();
        public ObservableCollection<PurchaserBind> Purchasers { get { return purchasers; } }

        private string maxPrice;
        [EntityProperty]
        [FloatNumber(0, 1000000000, IsNullable = true, ErrorMessage = "售价不合法")]
        public string MaxPrice
        {
            get { return maxPrice; }
            set
            {
                maxPrice = value;
                this.Validate("MaxPrice");
            }
        }

        private string minPrice;
        [EntityProperty]
        [FloatNumber(0, 1000000000, IsNullable = true, ErrorMessage = "售价不合法")]
        public string MinPrice
        {
            get { return minPrice; }
            set
            {
                minPrice = value;
                this.Validate("MinPrice");
            }
        }

        private string maxAmount;
        [EntityProperty]
        [FloatNumber(0, 10000000, IsNullable = true, ErrorMessage = "最大重量不合法")]
        public string MaxAmount
        {
            get { return maxAmount; }
            set
            {
                maxAmount = value;
                this.Validate("MaxAmount");
            }
        }

        private string minAmount;
        [EntityProperty]
        [FloatNumber(0, 10000000, IsNullable = true, ErrorMessage = "最小重量不合法")]
        public string MinAmount
        {
            get { return minAmount; }
            set
            {
                minAmount = value;
                this.Validate("MinAmount");
            }
        }




        private string purchaserId;
        [EntityProperty]
        public string PurchaserId
        {
            get { return purchaserId; }
            set
            {
                purchaserId = value;
                this.RaisePropertyChanged("PurchaserId");
            }
        }

        private DateTime? startOperationDate;
        [EntityProperty]
        public DateTime? StartOperationDate
        {
            get { return startOperationDate; }
            set
            {
                startOperationDate = value;
                this.RaisePropertyChanged("StartOperationDate");
            }
        }
        private DateTime? endOperationDate;
        [EntityProperty]
        public DateTime? EndOperationDate
        {
            get { return endOperationDate; }
            set
            {
                endOperationDate = value;
                this.RaisePropertyChanged("EndOperationDate");
            }
        }

        private string principalId;
        [EntityProperty]

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
        #endregion

        #region 列表数据
        public IEnumerable<SellWool> SellWoolList { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action Initialize = () =>
            {

                int count;
                this.SellWoolList = this.Service.GetSellWool(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<SellWool>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), SellWoolList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);

        }
        #endregion

        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddSellWoolWindow win = new AddSellWoolWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetSellWool(this.Filter, rowCount).ToArray();
        }
    }
}
