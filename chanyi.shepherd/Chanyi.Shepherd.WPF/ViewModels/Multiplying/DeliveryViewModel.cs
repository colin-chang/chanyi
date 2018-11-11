using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Multiplying;
using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Multiplying;
using Chanyi.Shepherd.WPF.Model;

namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class DeliveryViewModel : ListViewModel
    {
        public DeliveryViewModel(bool withinInitilization) { }
        public DeliveryViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;
            Mapper.CreateMap<DeliveryViewModel, DeliveryFilter>();
            Mapper.CreateMap<Delivery, DeliveryData>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var females = this.Service.GetDeliverySheepSelectBind();
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
               {
                   this.Females.Clear();
                   this.Principals.Clear();
                   this.Females.Add(new SheepBind { SerialNumber = defaultSelection });
                   females.ForEach(f => this.Females.Add(f));
                   this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                   principals.ForEach(p => this.Principals.Add(p));
                   this.Reset();
               }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public DeliveryFilter Filter
        {
            get
            {
                DeliveryFilter filter = Mapper.Map<DeliveryFilter>(this);
                return filter;
            }
        }

        #region 数据源及绑定
        private ObservableCollection<SheepBind> females = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Females { get { return females; } }

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

        private DeliveryWayEnum? deliveryWay;
        [EntityProperty]
        public DeliveryWayEnum? DeliveryWay
        {
            get { return deliveryWay; }
            set
            {
                deliveryWay = value;
                this.RaisePropertyChanged("DeliveryWay");
            }
        }

        private List<object> numbers = new List<object>();
        public List<object> Numbers
        {
            get
            {
                if (numbers.Count() <= 0)
                {
                    numbers.Add(new { Name = defaultSelection });
                    for (int i = 0; i < 10; i++)
                        numbers.Add(new { Id = i, Name = i });
                }
                return numbers;
            }
        }

        private int? liveMaleCount;
        [EntityProperty]
        public int? LiveMaleCount
        {
            get { return liveMaleCount; }
            set
            {
                liveMaleCount = value;
                this.RaisePropertyChanged("LiveMaleCount");
            }
        }

        private int? liveFemaleCount;
        [EntityProperty]
        public int? LiveFemaleCount
        {
            get { return liveFemaleCount; }
            set
            {
                liveFemaleCount = value;
                this.RaisePropertyChanged("LiveFemaleCount");
            }
        }

        private int? liveTotalCount;
        [EntityProperty]
        public int? LiveTotalCount
        {
            get { return liveTotalCount; }
            set
            {
                liveTotalCount = value;
                this.RaisePropertyChanged("LiveTotalCount");
            }
        }

        private int? totalDeliveryCount;
        /// <summary>
        /// TotlCount与分页条重名，在此重命名
        /// </summary>
        [EntityProperty]
        public int? TotalDeliveryCount
        {
            get { return totalDeliveryCount; }
            set
            {
                totalDeliveryCount = value;
                this.RaisePropertyChanged("TotalDeliveryCount");
            }
        }

        private DateTime? startdeliveryDate;
        [EntityProperty]
        public DateTime? StartDeliveryDate
        {
            get { return startdeliveryDate; }
            set
            {
                startdeliveryDate = value;
                this.RaisePropertyChanged("StartDeliveryDate");
            }
        }

        private DateTime? endDeliveryDate;
        [EntityProperty]
        public DateTime? EndDeliveryDate
        {
            get { return endDeliveryDate; }
            set
            {
                endDeliveryDate = value;
                this.RaisePropertyChanged("EndDeliveryDate");
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
        public IEnumerable<DeliveryData> DeliveryList { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action InitializeDelivery = () =>
            {
                int count;
                this.DeliveryList = this.Service.GetDelivery(this.Filter, this.PageIndex, this.PageSize, out count).Select(s => Mapper.Map<DeliveryData>(s));
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<Delivery>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.DeliveryList);
            };
            this.ProgressRing.Show();
            InitializeDelivery.BeginInvoke(ar => InitializeDelivery.EndInvoke(ar as IAsyncResult), InitializeDelivery);
        }
        #endregion


        public DelegateCommand<string> EditCommand
        {
            get
            {
                return this.GetEditCommand(id =>
                {
                    EditDeliveryWindow win = new EditDeliveryWindow(id);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        this.LoadData();
                });
            }
        }

        public DelegateCommand<string> RemoveCommand
        {
            get
            {
                return this.GetRemoveCommand(id =>
                {
                    this.Service.DeleteDelivery(id);
                    this.LoadData();
                });
            }
        }
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    AddDeliveryWindow win = new AddDeliveryWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            this.DeliveryList = this.Service.GetDelivery(this.Filter, rowCount).Select(s => Mapper.Map<DeliveryData>(s));
            return this.DeliveryList.ToArray();
        }
    }
}
