using Chanyi.Shepherd.WPF.ViewModels.ReportForms;
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

namespace Chanyi.Shepherd.WPF.Views.ReportForms
{
    /// <summary>
    /// FinanceReportUC.xaml 的交互逻辑
    /// </summary>
    public partial class FinanceReportUC : UserControl
    {
        public FinanceReportUC()
        {
            InitializeComponent();
        }
        public FinanceReportUC(string header, string icon, string intro)
            : this()
        {
            this.DataContext = new FinanceReportViewModel(header, icon, intro);
        }
    }
}
