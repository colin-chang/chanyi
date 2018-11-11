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

using Chanyi.Shepherd.WPF.ViewModels.Assist;

namespace Chanyi.Shepherd.WPF.Views.Assist
{
    /// <summary>
    /// TwoMatingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TwoMatingWindow : Window
    {
        public TwoMatingWindow()
        {
            InitializeComponent();
           this.DataContext = new TwoMatingViewModel();
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox cb = e.Source as ComboBox;
            if (cb != null && cb.IsEditable)
                this.ResetComboBox(cb);
        }
    }
}
