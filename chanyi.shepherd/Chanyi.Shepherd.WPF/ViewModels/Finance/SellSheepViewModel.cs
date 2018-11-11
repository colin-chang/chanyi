using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.QueryModel.Model.Finance;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Finance;
using System.Collections.ObjectModel;
using Chanyi.Shepherd.WPF.Views.GroupManage;



namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class SellSheepViewModel : ListViewModel
    {
        public SellSheepViewModel(bool withinInitilization) { }
        public SellSheepViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<SellSheepViewModel, SellSheepBatchFilter>();
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

        public SellSheepBatchFilter Filter
        {
            get
            {
                SellSheepBatchFilter filter = Mapper.Map<SellSheepBatchFilter>(this);
                return filter;
            }
        }

        #region 绑定搜索字段

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private ObservableCollection<PurchaserBind> purchasers = new ObservableCollection<PurchaserBind>();
        public ObservableCollection<PurchaserBind> Purchasers { get { return purchasers; } }

        private string batchId;
        [EntityProperty]
        public string BatchId
        {
            get { return batchId; ; }
            set
            {
                batchId = value;
                this.RaisePropertyChanged("BatchId");
            }
        }

        private string serialNumber;
        [EntityProperty]
        public string SerialNumber
        {
            get { return serialNumber; }
            set
            {
                serialNumber = value;
                this.RaisePropertyChanged("SerialNumber");
            }
        }

        private string minPrice;
        [EntityProperty]
        public string MinPrice
        {
            get { return minPrice; }
            set
            {
                minPrice = value;
                this.RaisePropertyChanged("MinPrice");
            }
        }
        private string maxPrice;
        [EntityProperty]
        public string MaxPrice
        {
            get { return maxPrice; }
            set
            {
                maxPrice = value;
                this.RaisePropertyChanged("MaxPrice");
            }
        }

        private string minTotalWeight;
        [EntityProperty]
        public string MinTotalWeight
        {
            get { return minTotalWeight; }
            set
            {
                minTotalWeight = value;
                this.RaisePropertyChanged("MinTotalWeight");
            }
        }
        private string maxTotalWeight;
        [EntityProperty]
        public string MaxTotalWeight
        {
            get { return maxTotalWeight; }
            set
            {
                maxTotalWeight = value;
                this.RaisePropertyChanged("MaxTotalWeight");
            }
        }

        private string minSellCount;
        [EntityProperty]
        public string MinSellCount
        {
            get { return minSellCount; }
            set
            {
                minSellCount = value;
                this.RaisePropertyChanged("MinSellCount");
            }
        }
        private string maxSellCount;
        [EntityProperty]
        public string MaxSellCount
        {
            get { return maxSellCount; }
            set
            {
                maxSellCount = value;
                this.RaisePropertyChanged("MaxSellCount");
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
        public IEnumerable<SellSheepBatch> SellSheepBatchList { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action Initialize = () =>
            {
                //SellSheepBatchFilter
                int count;
                this.SellSheepBatchList = this.Service.GetSellSheepBath(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<SellSheepBatch>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), SellSheepBatchList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);

        }
        #endregion

        #region 选中卖出批次显示所有的羊
        private List<SellSheep> allSellSheeps;
        public List<SellSheep> AllSellSheeps
        {
            get { return allSellSheeps; }
            set
            {
                allSellSheeps = value;
                this.RaisePropertyChanged("AllSellSheeps");
            }
        }
        private int pageIndexSheep;

        public int PageIndexSheep
        {
            get { return pageIndexSheep < 1 ? 1 : pageIndexSheep; }
            set
            {
                pageIndexSheep = value;
                this.RaisePropertyChanged("PageIndexSheep");
            }
        }
        private int pageSizeSheep = 10;
        private int totalCountSheep;

        public virtual int PageSizeSheep { get { return this.pageSizeSheep; } }

        public int TotalCountSheep
        {
            get { return totalCountSheep; }
            set
            {
                totalCountSheep = value;
                this.RaisePropertyChanged("TotalCountSheep");
            }
        }
        public DelegateCommand<string> SelectSellSheepCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        if (this.SellSheepBatchList.Count() <= 0)
                            return;
                        id = this.SellSheepBatchList.FirstOrDefault().Id;
                    }
                    this.selectedBatchId = id;
                    LoadSellSheep(id);
                });
            }
        }
        private  string selectedBatchId { get; set; }
        private void LoadSellSheep(string id)
        {
            int count;
            this.AllSellSheeps = this.Service.GetSellSheep(id, this.PageIndexSheep, this.pageSizeSheep, out count);
            this.TotalCountSheep = count;
        }

        public DelegateCommand PageSheepChangedCommand { get { return new DelegateCommand(() => LoadSellSheep(this.selectedBatchId)); } }
        #endregion

        public new DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddSellSheepWindow win = new AddSellSheepWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        public DelegateCommand AddAllCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddSellSheepAllWindow win = new AddSellSheepAllWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        LoadData();
                    }
                });
            }
        }

        public DelegateCommand SellSheepMonitoringCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SellSheepMonitoringWindow win = new SellSheepMonitoringWindow();
                    win.Owner = this.CurrentWindow;
                    if (win.ShowDialog() == true)
                        return;
                });
            }
        }

        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetSellSheepBath(this.Filter, rowCount).ToArray();
        }
    }
}
