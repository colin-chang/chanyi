using Chanyi.Shepherd.WPF.ViewModels.Tool;
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

namespace Chanyi.Shepherd.WPF.Views.Tool
{
    /// <summary>
    /// FeedParameterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FeedParameterWindow : Window
    {
        public FeedParameterWindow()
        {
            InitializeComponent();
            this.DataContext = new FeedParameterViewModel();
        }
    }
}
