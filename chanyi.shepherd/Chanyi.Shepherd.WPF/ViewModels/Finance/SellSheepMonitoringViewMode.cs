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
using System.Windows;

namespace Chanyi.Shepherd.WPF.ViewModels.GroupManage
{
    class SellSheepMonitoringViewMode : BaseViewModel
    {
        protected string defaultSelection = ConfigurationManager.AppSettings["listDefaultSelection"];

        public SellSheepMonitoringViewMode()
        {
            InitializeBindData();
        }

        public SellSheepMonitoringViewMode(bool withinInitilization) { }

        protected override void InitializeBindData()
        {
            LoaddingWindow win = new LoaddingWindow { Owner = this.CurrentWindow };
            Action load = () =>
            {
                Action close = () => this.UIDispatcher.Invoke(new Action(() => win.Close()), DispatcherPriority.Send, null);

                Dictionary<string, long> dict = new Dictionary<string, long>();

                this.Service.GetPeriodsSellSheepCount(this.StartDate, this.EndDate).ForEach(gc => dict[(gc.Month).ToString()] = gc.Count);
                this.ChartData = dict;
                if (isReload)
                {
                    close();
                    return;
                }
                close();
            };
            load.BeginInvoke(ar => load.EndInvoke(ar as IAsyncResult), load);
            win.ShowDialog();
        }

        private DateTime? startDate;
        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                this.RaisePropertyChanged("StartDate");
            }
        }

        private DateTime endDate = DateTime.Today;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                this.RaisePropertyChanged("EndDate");
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

        bool isReload = false;

        public DelegateCommand ReloadCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if ((DateTime)this.StartDate > this.EndDate)
                    {
                        MessageBox.Show(Application.Current.MainWindow, "时间段选择有误!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    this.isReload = true;
                    this.InitializeBindData();
                });
            }
        }
    }
}
