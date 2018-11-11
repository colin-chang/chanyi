
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

using Chanyi.Shepherd.WPF.UserControls;
using Chanyi.Shepherd.QueryModel.Model.ReportForms;
using Chanyi.Shepherd.WPF.ViewModels.ReportForms;
using System.Threading;
using System.Globalization;

namespace Chanyi.Shepherd.WPF.Views.ReportForms
{
    /// <summary>
    /// FeedReportUC.xaml 的交互逻辑
    /// </summary>
    public partial class FeedReportUC : UserControl
    {
        public FeedReportUC()
        {
            InitializeComponent();

            //Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM";
        }
        public FeedReportUC(string header, string icon, string intro)
            : this()
        {
            this.DataContext = new FeedReportViewModel(header, icon, intro, this.spContent);
        }
    }
}
