using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Chanyi.Shepherd.QueryModel.Model.ReportForms;

using System.ComponentModel;

namespace Chanyi.Shepherd.WPF.UserControls
{
    /// <summary>
    /// FeedChartReport.xaml 的交互逻辑
    /// </summary>
    public partial class FeedChartReport : UserControl
    {
        public FeedChartReport()
        {
            InitializeComponent();
        }

        public IEnumerable<FeedReport> ChartData { get; set; }

        public FeedChartReport(IEnumerable<FeedReport> _chartData, string _feedName)
            : this()
        {
            //this.ChartData = _chartData;
            this.dg.DataContext = _chartData;

            this.tblFeedName.Text = _feedName;
            //this.tblTotalUsed.Text = _chartData.Sum(t => t.Used).ToString();


            Window win = Application.Current.MainWindow;
            double _mainWidth = win.ActualWidth - 250 - 250;

            if (_mainWidth > 900)
            {
                this.Width = _mainWidth;
                this.exp.Width = _mainWidth-40;
                this.chartCol.Width = this.Width - (920 - 730);
            }
        }

    }
}
