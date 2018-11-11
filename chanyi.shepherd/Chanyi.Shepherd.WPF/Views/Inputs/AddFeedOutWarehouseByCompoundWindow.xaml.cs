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
using Chanyi.Shepherd.WPF.ViewModels.Inputs;

namespace Chanyi.Shepherd.WPF.Views.Inputs
{
    /// <summary>
    /// AddFeedOutWarehouseByCompoundWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddFeedOutWarehouseByCompoundWindow : Window
    {
        public AddFeedOutWarehouseByCompoundWindow()
        {
            InitializeComponent();
            this.DataContext = new AddFeedOutWarehouseByCompoundViewModel(this.progress);
        }
    }
}
