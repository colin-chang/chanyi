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
    class BuySheepViewModel : ListViewModel
    {
        public BuySheepViewModel(bool withinInitilization) { }

        public BuySheepViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<BuySheepViewModel, BuySheepFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var sheeps = this.Service.GetBuySheepBind4Select();
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Sheeps.Clear();
                    this.Principals.Clear();

                    this.Sheeps.Add(new SheepBind { SerialNumber = defaultSelection });
                    sheeps.ForEach(s => this.Sheeps.Add(s));
                    this.Principals.Add(new EmployeeBind { Name = this.defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));

                    this.Reset();
                }), null);
                this.Reset();
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public BuySheepFilter Filter
        {
            get
            {
                BuySheepFilter filter = Mapper.Map<BuySheepFilter>(this);
                return filter;
            }
        }

        #region 绑定搜索字段

        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();

        public ObservableCollection<SheepBind> Sheeps
        {
            get { return sheeps; }
        }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string sheepId;
        [EntityProperty]
        public string SheepId
        {
            get { return sheepId; }
            set
            {
                sheepId = value;
                this.RaisePropertyChanged("SheepId");
            }
        }

        private string maxMoney;
        [EntityProperty]
        [FloatNumber(0, 1000000000, IsNullable = true, ErrorMessage = "购羊费用输入不合法")]
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
        [FloatNumber(0, 1000000000, IsNullable = true, ErrorMessage = "购羊费用不合法")]
        public string MinMoney
        {
            get { return minMoney; }
            set
            {
                minMoney = value;
                this.Validate("MinMoney");
            }
        }

        private string source;
        [EntityProperty]
        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                this.RaisePropertyChanged("Source");
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
        public IEnumerable<BuySheep> BuySheepList { get; set; }
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
                this.BuySheepList = this.Service.GetBuySheep(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<BuySheep>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), BuySheepList);
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
                    AddBuySheepWindow win = new AddBuySheepWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetBuySheep(this.Filter, rowCount).ToArray();
        }

    }
}

