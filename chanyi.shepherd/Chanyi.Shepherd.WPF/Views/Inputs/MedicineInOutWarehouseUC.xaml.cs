using Chanyi.Shepherd.WPF.ViewModels.Inputs;
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

namespace Chanyi.Shepherd.WPF.Views.Inputs
{
    /// <summary>
    /// MedicineInWarehouseUC.xaml 的交互逻辑
    /// </summary>
    public partial class MedicineInOutWarehouseUC : UserControl
    {
        public MedicineInOutWarehouseUC()
        {
            InitializeComponent();
        }
        public MedicineInOutWarehouseUC(string header, string icon, string intro)
            : this()
        {
            this.DataContext = new MedicineInputsViewModel(header, icon, intro);
        }
        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox cb = e.Source as ComboBox;
            if (cb != null && cb.IsEditable) this.SetComboBox(cb);
        }
    }
}
