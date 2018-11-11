using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Xml.Linq;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.Model.Chart;
using Chanyi.Shepherd.WPF.Expands.Converter;
using Chanyi.Shepherd.WPF.Helper;
using Chanyi.Shepherd.WPF.Views;
using System.IO;

namespace Chanyi.Shepherd.WPF.ViewModels
{
    class StartViewModel : BaseViewModel
    {
        public StartViewModel()
        {
            InitializeBindData();
        }

        protected override void InitializeBindData()
        {
            Action initialize = () =>
            {
                InitChartData();
                InitNews();
            };
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);
        }

        void InitChartData()
        {
            Dictionary<string, long> dict = new Dictionary<string, long>();
            var cvt = new GrowthStage2StringConverter();
            this.Service.GetPeriodsSheepGrowthStageCount(DateTime.Today, null).ForEach(gc => dict[cvt.Convert(gc.GrowthStage, typeof(string), null, null).ToString()] = gc.Count);
            this.ChartData = dict;

            Dictionary<string, string> dicp = new Dictionary<string, string>();
            long sum = dict.Values.Sum();
            dict.Keys.ToList().ForEach(k => dicp[k] = dict[k] <= 0 ? "0%" : Math.Round((dict[k] * 1.0 / sum * 100), 2) + "%");
            this.PiePercent = dicp;
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
                this.GroupStructure = new Dictionary<string, long>();
                value.ToList().ForEach(kv => this.GroupStructure.Add(kv.Key, kv.Value));
                this.GroupStructure.Add("总计", this.ChartData.Select(c => c.Value).Sum());
                this.RaisePropertyChanged("ChartData");
                this.RaisePropertyChanged("GroupStructure");
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

        public Dictionary<string, long> GroupStructure { get; set; }

        public string Version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        private ObservableCollection<string> news = new ObservableCollection<string>();
        public ObservableCollection<string> News { get { return news; } }

        void InitNews()
        {
            var news = XmlHelper.GetNews();
            this.UIDispatcher.Invoke(new Action(() => news.ForEach(n => this.News.Add(n))), null);
        }

        public DelegateCommand NewsCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    LoaddingWindow loadding = new LoaddingWindow();
                    //关闭加载窗口
                    Action loaded = () => this.UIDispatcher.Invoke(new Action(() => loadding.Close()), null);

                    Action load = () =>
                    {
                        bool handed = false;
                        if (this.NetworkAvailable)
                        {
                            string ip = ConfigurationManager.AppSettings["supportHost"];
                            try
                            {
                                PingReply reply = new Ping().Send(ip);
                                if (reply.Status == IPStatus.Success)
                                {
                                    loaded();
                                    Process.Start(ConfigurationManager.AppSettings["newsURL"]);
                                    handed = true;
                                }
                            }
                            catch
                            {
                                handed = false;
                            }
                        }
                        if (!handed)
                        {
                            loaded();
                            Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\ReadMe.txt"));
                        }
                    };
                    load.BeginInvoke(ar => load.EndInvoke(ar as IAsyncResult), load);
                    loadding.ShowDialog();
                });
            }
        }

        public DelegateCommand ViewDocumentCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    (Application.Current.MainWindow.DataContext as MainViewModel).ViewDocument();
                });
            }
        }
    }
}
