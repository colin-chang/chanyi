using System;
using System.Collections.Generic;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Inputs;
using System.Windows;
using System.Collections.ObjectModel;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class FeedInputsViewModel : ListViewModel
    {
        public FeedInputsViewModel(bool withinInitilization) { }

        public FeedInputsViewModel(string header, string icon, string intro,InOutWarehouseDirectionEnum direction)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.Direction = direction;
            Mapper.CreateMap<FeedInputsViewModel, FeedInOutFilter>();
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
                    feedNames.ForEach(f => this.FeedNames.Add(f));
                    this.TypeNames.Add(new FeedTypeBind { Name = defaultSelection });
                    typeNames.ForEach(t => this.TypeNames.Add(t));
                    this.AreaNames.Add(new AreaBind { Name = defaultSelection });
                    areaNames.ForEach(a => this.AreaNames.Add(a));
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
        public FeedInOutFilter Filter
        {
            get
            {
                FeedInOutFilter filter = Mapper.Map<FeedInOutFilter>(this);
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

        private InOutWarehouseDirectionEnum? direction;
    
        public InOutWarehouseDirectionEnum? Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                this.RaisePropertyChanged("Direction");
            }
        }

        private OutWarehouseDispositonEnum? dispositon;
        [EntityProperty]
        public OutWarehouseDispositonEnum? Dispositon
        {
            get { return dispositon; }
            set
            {
                dispositon = value;
                this.RaisePropertyChanged("Dispositon");
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
        public List<FeedInOut> FeedInList { get; set; }
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
                this.FeedInList = this.Service.GetFeedInOut(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<FeedInOut>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.FeedInList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
        #endregion

        public DelegateCommand AddInCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddFeedInOutWarehouseWindow win = new AddFeedInOutWarehouseWindow(InOutWarehouseDirectionEnum.In) { Owner = Application.Current.MainWindow };
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        public DelegateCommand AddOutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddFeedInOutWarehouseWindow win = new AddFeedInOutWarehouseWindow(InOutWarehouseDirectionEnum.Out) { Owner = Application.Current.MainWindow };
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        public DelegateCommand AddCustomCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddFeedOutWarehouseWindow win = new AddFeedOutWarehouseWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        public DelegateCommand AddCompoundCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddFeedOutWarehouseByCompoundWindow win = new AddFeedOutWarehouseByCompoundWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddFeedWindow win = new AddFeedWindow();
                    win.Owner = CurrentWindow;
                    win.ShowDialog();
                });
            }
        }

        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetFeedInOut(this.Filter, rowCount).ToArray();
        }
    }
}
