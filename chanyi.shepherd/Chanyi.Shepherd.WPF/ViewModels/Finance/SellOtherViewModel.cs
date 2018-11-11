using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Finance;
using Chanyi.Shepherd.QueryModel.Model.Finance;

namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class SellOtherViewModel : ListViewModel
    {
        public SellOtherViewModel(bool withinInitilization) { }

        public SellOtherViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<SellOtherViewModel, SellOtherFilter>();
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

        public SellOtherFilter Filter
        {
            get
            {
                SellOtherFilter filter = Mapper.Map<SellOtherFilter>(this);
                return filter;
            }
        }

        #region 绑定搜索字段

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private ObservableCollection<PurchaserBind> purchasers = new ObservableCollection<PurchaserBind>();
        public ObservableCollection<PurchaserBind> Purchasers { get { return purchasers; } }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        private string maxPrice;
        [EntityProperty]
        [FloatNumber(0, int.MaxValue, IsNullable = true, ErrorMessage = "最大价格不合法")]
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
        [FloatNumber(0, int.MaxValue, IsNullable = true, ErrorMessage = "最小价格不合法")]
        public string MinPrice
        {
            get { return minPrice; }
            set
            {
                minPrice = value;
                this.Validate("MinPrice");
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
       public IEnumerable<SellOther> SellOtherList { get; set; }
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
                this.SellOtherList = this.Service.GetSellOther(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<SellOther>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.SellOtherList);
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
                    AddSellOtherWindow win = new AddSellOtherWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetSellOther(this.Filter, rowCount).ToArray();
        }
    }
}