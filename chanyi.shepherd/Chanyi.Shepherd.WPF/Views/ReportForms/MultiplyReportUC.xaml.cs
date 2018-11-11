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

using Chanyi.Shepherd.WPF.ViewModels.ReportForms;

namespace Chanyi.Shepherd.WPF.Views.ReportForms
{
    /// <summary>
    /// 繁殖报表
    /// </summary>
    public partial class MultiplyReportUC : UserControl
    {
        public MultiplyReportUC()
        {
            InitializeComponent();
        }
        public MultiplyReportUC(string header, string icon, string intro)
            : this()
        {
            this.DataContext = new MultiplyReportViewModel(header, icon, intro);
        }
    }
}
