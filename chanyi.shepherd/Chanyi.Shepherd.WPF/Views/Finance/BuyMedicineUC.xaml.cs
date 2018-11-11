using Chanyi.Shepherd.WPF.ViewModels.Finance;
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

namespace Chanyi.Shepherd.WPF.Views.Finance
{
    /// <summary>
    /// BuyMedicineUC.xaml 的交互逻辑
    /// </summary>
    public partial class BuyMedicineUC : UserControl
    {
        public BuyMedicineUC()
        {
            InitializeComponent();
            this.stkProp.Children.OfType<StackPanel>().ToList().ForEach(s =>
            {
                s.Style = Resources["stkProp"] as Style;
                var txtblc = s.Children[0] as TextBlock;
                if (txtblc != null)
                {
                    txtblc.Style = Resources["txtblcProp"] as Style;
                    txtblc.ToolTip = txtblc.Text;
                }
            });
        }
        public BuyMedicineUC(string header, string icon, string intro)
            : this()
        {
            this.DataContext = new BuyMedicineViewModel(header, icon, intro);
        }
        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox cb = e.Source as ComboBox;
            if (cb != null && cb.IsEditable) this.SetComboBox(cb);
        }
    }
}
