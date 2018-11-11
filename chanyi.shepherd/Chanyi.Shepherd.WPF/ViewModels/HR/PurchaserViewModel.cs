using System;
using System.Collections.Generic;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;


using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.HR;
using Chanyi.Shepherd.QueryModel.Model.HR;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.HR;
using System.Collections.ObjectModel;

namespace Chanyi.Shepherd.WPF.ViewModels.HR
{
    class PurchaserViewModel : ListViewModel
    {
        public PurchaserViewModel(bool withinInitilization) { }
        public PurchaserViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;
            Mapper.CreateMap<PurchaserViewModel, CooperaterFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var names = this.Service.GetPurchaserBind();
                this.UIDispatcher.Invoke(new Action(() => {
                    this.Names.Clear();
                    this.Names.Add(new PurchaserBind { Name = defaultSelection });
                    names.ForEach(n => this.Names.Add(n));
                    this.Reset();
                }), null);
               
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public CooperaterFilter Filter
        {
            get
            {
                CooperaterFilter filter = Mapper.Map<CooperaterFilter>(this);
                return filter;
            }
        }

        private ObservableCollection<PurchaserBind> names = new ObservableCollection<PurchaserBind>();
        public ObservableCollection<PurchaserBind> Names { get { return names; } }

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
        public List<Cooperater> PurchaserList { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action initialize = () =>
            {
                int count;
                this.PurchaserList = this.Service.GetCooperater(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<Cooperater>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.PurchaserList);
            };
            this.ProgressRing.Show();
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);
        }
        public DelegateCommand<string> EditCommand
        {
            get
            {
                return this.GetEditCommand(id =>
                {
                    EditPurchaserWindow win = new EditPurchaserWindow(id);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        this.LoadData();
                    }
                });
            }
        }

        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddPurchaserWindow win = new AddPurchaserWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetCooperater(this.Filter, rowCount).ToArray();
        }
    }
}
