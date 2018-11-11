using System;
using System.Collections.Generic;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.QueryModel.Model.Finance;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Finance;
using System.Collections.ObjectModel;

namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class BuyOtherViewModel : ListViewModel
    {
        public BuyOtherViewModel(bool withinInitilization) { }

        public BuyOtherViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            Mapper.CreateMap<BuyOtherViewModel, BuyOtherFilter>();
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

        public BuyOtherFilter Filter
        {
            get
            {
                BuyOtherFilter filter = Mapper.Map<BuyOtherFilter>(this);
                return filter;
            }
        }
        #region 搜索字段
        private string name;
        [EntityProperty]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        private string maxMoney;
        [EntityProperty]
        public string MaxMoney
        {
            get { return maxMoney; }
            set
            {
                maxMoney = value;
                this.RaisePropertyChanged("MaxMoney");
            }
        }
        private string minMoney;
        [EntityProperty]
        public string MinMoney
        {
            get { return minMoney; }
            set
            {
                minMoney = value;
                this.RaisePropertyChanged("MinMoney");
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

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

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
        #endregion

        #region 列表数据
        public List<BuyOther> OtherList { get; set; }
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
                this.OtherList = this.Service.GetBuyOther(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<BuyOther>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.OtherList);
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
                    AddBuyOtherWindow win = new AddBuyOtherWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        LoadData();
                    }
                });
            }
        }

        #region 右边栏展示选中
        private OtherInOut other;
        public OtherInOut Other
        {
            get { return other; }
            set
            {
                other = value;
                this.RaisePropertyChanged("Other");
            }
        }

        public DelegateCommand<string> SelectCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    //this.Other = this.Service.GetOtherInOutDetail(id);
                    this.Other = this.Service.GetOtherInOutDetailById(id);
                });
            }
        } 
        #endregion
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetBuyOther(this.Filter, rowCount).ToArray();
        }
    }
}


