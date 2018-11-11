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

using Chanyi.Shepherd.WPF.ViewModels.Finance;

namespace Chanyi.Shepherd.WPF.Views.Finance
{
    /// <summary>
    /// AddSellFeedWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddSellFeedWindow : Window
    {
        public AddSellFeedWindow()
        {
            InitializeComponent();
            this.DataContext = new AddSellFeedViewModel();
        }
    }
}
