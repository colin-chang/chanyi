﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Finance;
using Chanyi.Shepherd.QueryModel.Model.Finance;
using Chanyi.Shepherd.QueryModel.Model.Input;
namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class SellFeedViewModel: ListViewModel
    {
        public SellFeedViewModel(bool withinInitilization) { }
        public SellFeedViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            Mapper.CreateMap<SellFeedViewModel, SellFeedFilter>();
            InitializeBindData();
        }

        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var feedNames = this.Service.GetFeedNameBind();
                var typeNames = this.Service.GetFeedTypeBind();
                var areaNames = this.Service.GetAreaBind();
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.FeedNames.Clear();
                    this.TypeNames.Clear();
                    this.AreaNames.Clear();
                    this.Principals.Clear();

                    this.FeedNames.Add(new FeedNameBind { Name = defaultSelection });
                    this.TypeNames.Add(new FeedTypeBind { Name = defaultSelection });
                    this.AreaNames.Add(new AreaBind { Name = defaultSelection });
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });

                    feedNames.ForEach(f => this.FeedNames.Add(f));
                    typeNames.ForEach(t => this.TypeNames.Add(t));
                    areaNames.ForEach(a => this.AreaNames.Add(a));
                    principals.ForEach(p => this.Principals.Add(p));

                    this.Reset();
                }), null);

            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public SellFeedFilter Filter
        {
            get
            {
                SellFeedFilter filter = Mapper.Map<SellFeedFilter>(this);
                return filter;
            }
        }

        #region 搜索字段
        private ObservableCollection<FeedNameBind> feedNames = new ObservableCollection<FeedNameBind>();         
        public ObservableCollection<FeedNameBind> FeedNames { get { return feedNames; } }

        private ObservableCollection<FeedTypeBind> typeNames = new ObservableCollection<FeedTypeBind>();
        public ObservableCollection<FeedTypeBind> TypeNames { get { return typeNames; } }
        private ObservableCollection<AreaBind> areaNames = new ObservableCollection<AreaBind>();
        public ObservableCollection<AreaBind> AreaNames { get { return areaNames; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }


        private string nameId;
        [EntityProperty]
        public string NameId
        {
            get { return nameId; }
            set
            {
                nameId = value;
                this.RaisePropertyChanged("NameId");
            }
        }
        private string typeId;
        [EntityProperty]
        public string TypeId
        {
            get { return typeId; ; }
            set
            {
                typeId = value;
                this.RaisePropertyChanged("TypeId");
            }
        }

        private string areaId;
        [EntityProperty]
        public string AreaId
        {
            get { return areaId; }
            set
            {
                areaId = value;
                this.RaisePropertyChanged("AreaId");
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
        public List<SellFeed> SellFeedList { get; set; }
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
                this.SellFeedList = this.Service.GetSellFeed(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<SellFeed>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.SellFeedList);
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
                    AddSellFeedWindow win = new AddSellFeedWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        LoadData();
                    }
                });
            }
        }

      /// <summary>
      /// 选中饲料
      /// </summary>
        private FeedInOut feed;

        public FeedInOut Feed
        {
            get { return feed; }
            set
            {
                feed = value;
                this.RaisePropertyChanged("Feed");
            }
        }

        /// <summary>
        /// 展示选中饲料信息
        /// </summary>
        public DelegateCommand<string> SelectCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    this.Feed = this.Service.GetFeedInOutDetailById(id);
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetSellFeed(this.Filter, rowCount).ToArray();
        }
    }
}

