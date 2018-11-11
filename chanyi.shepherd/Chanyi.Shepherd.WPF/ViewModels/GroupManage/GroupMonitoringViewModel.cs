using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Configuration;
using System.Linq;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.WPF.Expands.Converter;
using Chanyi.Shepherd.WPF.Views;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.GroupManage
{
    class GroupMonitoringViewModel : BaseViewModel
    {
        protected string defaultSelection = ConfigurationManager.AppSettings["listDefaultSelection"];

        public GroupMonitoringViewModel()
        {
            InitializeBindData();
        }

        public GroupMonitoringViewModel(bool withinInitilization) { }

        protected override void InitializeBindData()
        {
            LoaddingWindow win = new LoaddingWindow { Owner = this.CurrentWindow };
            Action load = () =>
            {
                Action close = () => this.UIDispatcher.Invoke(new Action(() => win.Close()), DispatcherPriority.Send, null);

                Dictionary<string, long> dict = new Dictionary<string, long>();
                var cvt = new GrowthStage2StringConverter();
                this.Service.GetPeriodsSheepGrowthStageCount(this.Date, this.BreedId, this.Gender).ForEach(gc => dict[cvt.Convert(gc.GrowthStage, typeof(string), null, null).ToString()] = gc.Count);
                this.ChartData = dict;

                Dictionary<string, string> dicp = new Dictionary<string, string>();
                long sum = dict.Values.Sum();
                dict.Keys.ToList().ForEach(k => dicp[k] = dict[k] <= 0 ? "0%" : Math.Round((dict[k] * 1.0 / sum * 100), 2) + "%");
                this.PiePercent = dicp;

                if (isReload)
                {
                    close();
                    return;
                }

                var breeds = this.Service.GetBreedBind();
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Breeds.Clear();
                    this.Breeds.Add(new BreedBind { Name = defaultSelection });
                    breeds.ForEach(b => this.Breeds.Add(b));
                }), DispatcherPriority.Send, null);
                close();
            };
            load.BeginInvoke(ar => load.EndInvoke(ar as IAsyncResult), load);
            win.ShowDialog();
        }

        private DateTime date = DateTime.Today;

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                this.RaisePropertyChanged("Date");
            }
        }

        private GenderEnum? gender;

        public GenderEnum? Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                this.RaisePropertyChanged("Gender");
            }
        }

        private ObservableCollection<BreedBind> breeds = new ObservableCollection<BreedBind>();
        public ObservableCollection<BreedBind> Breeds { get { return breeds; } }

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

        private Dictionary<string, long> chartData;

        public Dictionary<string, long> ChartData
        {
            get
            {
                if (chartData == null)
                    chartData = new Dictionary<string, long>();
                return chartData;
            }
            set
            {
                chartData = value;
                this.RaisePropertyChanged("ChartData");
            }
        }

        private Dictionary<string, string> piePercent;

        public Dictionary<string, string> PiePercent
        {
            get { return piePercent; }
            set
            {
                piePercent = value;
                this.RaisePropertyChanged("PiePercent");
            }
        }

        bool isReload = false;

        public DelegateCommand ReloadCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.isReload = true;
                    this.InitializeBindData();
                });
            }
        }
    }
}
