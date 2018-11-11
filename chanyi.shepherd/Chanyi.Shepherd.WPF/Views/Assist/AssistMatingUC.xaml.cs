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
using Chanyi.Shepherd.WPF.ViewModels.Assist;

namespace Chanyi.Shepherd.WPF.Views.Assist
{
    /// <summary>
    /// AssistMatingUC.xaml 的交互逻辑
    /// </summary>
    public partial class AssistMatingUC : UserControl
    {
        public AssistMatingUC()
        {
            InitializeComponent();
        }
        public AssistMatingUC(string header, string icon, string intro)
            : this()
        {
            this.DataContext = new AssistMatingViewModel(header, icon, intro);
        }
        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox cb = e.Source as ComboBox;
            if (cb != null && cb.IsEditable) this.SetComboBox(cb);
        }
    }
}
