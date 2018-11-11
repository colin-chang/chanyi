using Chanyi.Shepherd.WPF.ViewModels.BaseInfo;
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

namespace Chanyi.Shepherd.WPF.Views.BaseInfo
{
    /// <summary>
    /// FarmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FarmWindow : Window
    {
        public FarmWindow()
        {
            InitializeComponent();
            this.DataContext = new FarmViewModel();
        }
    }
}
