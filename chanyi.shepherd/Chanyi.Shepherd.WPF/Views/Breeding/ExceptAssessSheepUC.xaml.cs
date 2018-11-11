
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

using Chanyi.Shepherd.WPF.ViewModels.Breeding;

namespace Chanyi.Shepherd.WPF.Views.Breeding
{
    /// <summary>
    /// 一键否决羊只
    /// </summary>
    public partial class ExceptAssessSheepUC : UserControl
    {
        public ExceptAssessSheepUC()
        {
            InitializeComponent();
        }

        public ExceptAssessSheepUC(string header, string icon, string intro)
            : this()
        {
            this.DataContext = new ExceptAssessSheepViewModel(header, icon, intro);
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox cb = e.Source as ComboBox;
            if (cb != null && cb.IsEditable) this.SetComboBox(cb);
        }
    }
}
