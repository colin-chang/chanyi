using Chanyi.Shepherd.WPF.ViewModels.Assist;
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

namespace Chanyi.Shepherd.WPF.Views.Assist
{
    /// <summary>
    /// FormulaNutrientUC.xaml 的交互逻辑
    /// </summary>
    public partial class FormulaNutrientUC : UserControl
    {
        public FormulaNutrientUC()
        {
            InitializeComponent();
        }

        public FormulaNutrientUC(string header, string icon, string intro) : this()
        {
            this.DataContext = new FormulaNutrientViewModel(header, icon, intro);
        }
    }
}
