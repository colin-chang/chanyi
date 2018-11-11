using Chanyi.Shepherd.WPF.ViewModels.File;
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
using System.Windows.Shapes;

namespace Chanyi.Shepherd.WPF.Views.File
{
    /// <summary>
    /// ExportWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ExportWindow : Window
    {
        public ExportWindow()
        {
            InitializeComponent();
            this.DataContext = new ExportViewModel();
        }

        public string FileName { get; set; }

        public int RowCount { get; set; }

        public bool CanExport { get; set; }

        public bool Exported { get; set; }
    }
}
