using System;
using System.Collections.Generic;

using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Collections.ObjectModel;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    public class FeedWarehouseViewModel : ListViewModel
    {
        public FeedWarehouseViewModel(bool withinInitilization) { }

        public FeedWarehouseViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            Mapper.CreateMap<FeedWarehouseViewModel, FeedInventoryFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var feedNames = this.Service.GetFeedNameBind();
                var typeNames = this.Service.GetFeedTypeBind();
                var areaNames = this.Service.GetAreaBind();
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.FeedNames.Clear();
                    this.TypeNames.Clear();
                    this.AreaNames.Clear();
                    this.FeedNames.Add(new FeedNameBind { Name = defaultSelection });
                    feedNames.ForEach(f => this.FeedNames.Add(f));
                    this.TypeNames.Add(new FeedTypeBind { Name = defaultSelection });
                    typeNames.ForEach(t => this.TypeNames.Add(t));
                    this.AreaNames.Add(new AreaBind { Name = defaultSelection });
                    areaNames.ForEach(a => this.AreaNames.Add(a));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public FeedInventoryFilter Filter
        {
            get
            {
                FeedInventoryFilter filter = Mapper.Map<FeedInventoryFilter>(this);
                return filter;
            }
        }

        #region 搜索字段
        private ObservableCollection<FeedNameBind> feedNames = new ObservableCollection<FeedNameBind>();
        public ObservableCollection<FeedNameBind> FeedNames { get { return feedNames; } }

        private ObservableCollection<FeedTypeBind> typeNames = new ObservableCollection<FeedTypeBind>();
        public ObservableCollection<FeedTypeBind> TypeNames { get { return typeNames; ; } }

        private ObservableCollection<AreaBind> areaNames = new ObservableCollection<AreaBind>();
        public ObservableCollection<AreaBind> AreaNames { get { return areaNames; } }

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

        private string maxAmount;
        [EntityProperty]
        public string MaxAmount
        {
            get { return maxAmount; }
            set
            {
                maxAmount = value;
                this.RaisePropertyChanged("MaxAmount");
            }
        }
        private string minAmount;
        [EntityProperty]
        public string MinAmount
        {
            get { return minAmount; }
            set
            {
                minAmount = value;
                this.RaisePropertyChanged("MinAmount");
            }
        }
        #endregion


        #region 列表
        public List<FeedInventory> FeedList { get; set; }
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
                this.FeedList = this.Service.GetFeedInventory(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<FeedInventory>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.FeedList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
        #endregion
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetFeedInventory(this.Filter, rowCount).ToArray();
        }
    }
}
