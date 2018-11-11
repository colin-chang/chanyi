using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;


using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.QueryModel.Model.Finance;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Finance;


namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class WaterRateViewModel : ListViewModel
    {
        public WaterRateViewModel(bool withinInitilization) { }

        public WaterRateViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<WaterRateViewModel, WaterRateFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {

            Action Initialize = () =>
            {
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Principals.Clear();

                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));
                this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public WaterRateFilter Filter
        {
            get
            {
                WaterRateFilter filter = Mapper.Map<WaterRateFilter>(this);
                return filter;
            }
        }

        #region 绑定搜索字段

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string maxMoney;
        [EntityProperty]
        [FloatNumber(0, int.MaxValue, IsNullable = true, ErrorMessage = "最大价格不合法")]
        public string MaxMoney
        {
            get { return maxMoney; }
            set
            {
                maxMoney = value;
                this.Validate("MaxMoney");
            }
        }

        private string minMoney;
        [EntityProperty]
        [FloatNumber(0, int.MaxValue, IsNullable = true, ErrorMessage = "最小价格不合法")]
        public string MinMoney
        {
            get { return minMoney; }
            set
            {
                minMoney = value;
                this.Validate("MinMoney");
            }
        }

        private string maxAmount;
        [EntityProperty]
        [FloatNumber(0, int.MaxValue, IsNullable = true, ErrorMessage = "最大数量不合法")]
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
        [FloatNumber(0, int.MaxValue, IsNullable = true, ErrorMessage = "最小数量不合法")]
        public string MinAmount
        {
            get { return minAmount; }
            set
            {
                minAmount = value;
                this.Validate("MinAmount");
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
        public IEnumerable<WaterRate> WaterRateList { get; set; }
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
                this.WaterRateList = this.Service.GetWaterRate(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<WaterRate>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.WaterRateList);
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
                    AddWaterRateWindow win = new AddWaterRateWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetWaterRate(this.Filter, rowCount).ToArray();
        }
    }
}
