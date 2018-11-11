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
using System.Windows.Shapes;

namespace Chanyi.Shepherd.WPF.Views.Assist
{
    /// <summary>
    /// DiseaseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DiseaseWindow : Window
    {
        public DiseaseWindow()
        {
            InitializeComponent();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var rich = sender as RichTextBox;
            rich.ScrollToHome();
        }

    }
}
