using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


using Chanyi.Shepherd.QueryModel.Model.ReportForms;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using AutoMapper;
using Chanyi.Shepherd.QueryModel.Filter.BaseInfo;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel;
using System.Windows.Threading;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;
using Microsoft.Practices.Prism.Commands;

namespace Chanyi.Shepherd.WPF.ViewModels.ReportForms
{
    class FeedSheepReportViewModel : ListViewModel
    {
        public FeedSheepReportViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<FeedSheepReportViewModel, SheepFilter>();
            InitializeBindData();
        }

        public FeedSheepReportViewModel(bool withinInitilization) { }

        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var allsheeps = this.Service.GetSheepBind(null);
                SheepBind defaultSheep = new SheepBind { SerialNumber = defaultSelection };
                var breeds = this.Service.GetBreedBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Sheeps.Clear();
                    this.Sheeps.Add(defaultSheep);
                    allsheeps.ForEach(s => this.Sheeps.Add(s));
                    this.Breeds.Clear();
                    this.Breeds.Add(new BreedBind { Name = defaultSelection });
                    breeds.ForEach(b => this.Breeds.Add(b));
                    this.Reset();
                }), DispatcherPriority.Send, null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public SheepFilter Filter
        {
            get
            {
                SheepFilter filter = Mapper.Map<SheepFilter>(this);
                return filter;
            }
        }

        #region 搜索绑定字段

        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }

        private ObservableCollection<BreedBind> breeds = new ObservableCollection<BreedBind>();
        public ObservableCollection<BreedBind> Breeds { get { return breeds; } }
        #endregion

        #region 搜索字段

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

        private string breedId;
        [EntityProperty]
        public string BreedId
        {
            get { return breedId; }
            set
            {
                breedId = value;
                this.RaisePropertyChanged("BreedId");
            }
        }

        private GenderEnum? gender;
        [EntityProperty]
        public GenderEnum? Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                this.RaisePropertyChanged("Gender");
            }
        }

        private GrowthStageEnum? growthStage;
        [EntityProperty]
        public GrowthStageEnum? GrowthStage
        {
            get { return growthStage; }
            set
            {
                growthStage = value;
                this.RaisePropertyChanged("GrowthStage");
            }
        }

        private OriginEnum? origin;
        [EntityProperty]
        public OriginEnum? Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                this.RaisePropertyChanged("Origin");
            }
        }
        private SheepStatusEnum? status;
        [EntityProperty]
        public SheepStatusEnum? Status
        {
            get { return status; }
            set
            {
                status = value;
                this.RaisePropertyChanged("Status");
            }
        }
        #endregion

        public IEnumerable<Sheep> SheepList { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action InitializeSheeps = () =>
            {
                int count;
                this.SheepList = this.Service.GetAllSheep(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<Sheep>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.SheepList);
            };
            this.ProgressRing.Show();
            InitializeSheeps.BeginInvoke(ar => InitializeSheeps.EndInvoke(ar as IAsyncResult), InitializeSheeps);
        }


        #region 显示食用饲料详情
        private ObservableCollection<FeedSheepReport> feedSheeps = new ObservableCollection<FeedSheepReport>();

        public ObservableCollection<FeedSheepReport> FeedSheeps { get { return feedSheeps; } }


        public DelegateCommand<string> SelectSheepCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    if (id == null) return;
                    var feeds = this.Service.GetFeedSheepReport(id);
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        this.FeedSheeps.Clear();
                        feeds.Each(f => this.FeedSheeps.Add(f));
                    }));
                });
            }
        }
        #endregion
    }
}

