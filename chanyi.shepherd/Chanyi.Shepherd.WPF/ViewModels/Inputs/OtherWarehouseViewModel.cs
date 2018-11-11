using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class OtherWarehouseViewModel : ListViewModel
    {

        public OtherWarehouseViewModel(bool withinInitilization) { }

        public OtherWarehouseViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<OtherWarehouseViewModel, OtherInventoryFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var names = this.Service.GetOtherBind();
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Names.Clear();
                    this.Names.Add(new OtherBind { Name = defaultSelection });
                    names.ForEach(f => this.Names.Add(f));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public OtherInventoryFilter Filter
        {
            get
            {
                OtherInventoryFilter filter = Mapper.Map<OtherInventoryFilter>(this);
                return filter;
            }
        }

        #region 搜索字段
        private ObservableCollection<OtherBind> names = new ObservableCollection<OtherBind>();
        public ObservableCollection<OtherBind> Names { get { return names; } }

        private string id;
        [EntityProperty]
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                this.RaisePropertyChanged("Id");
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

        #region 列表数据
        public List<OtherInventory> OtherInventoryList { get; set; }
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
                this.OtherInventoryList = this.Service.GetOtherInventory(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<OtherInventory>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.OtherInventoryList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
        #endregion
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetOtherInventory(this.Filter, rowCount).ToArray();
        }
    }
}

