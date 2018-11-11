using Chanyi.Shepherd.WPF.ViewModels.GroupManage;
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

namespace Chanyi.Shepherd.WPF.Views.GroupManage
{
    /// <summary>
    /// AddMoveSheepfoldWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddMoveSheepfoldWindow : Window
    {
        public AddMoveSheepfoldWindow()
        {
            InitializeComponent();
            (this.DataContext as AddMoveSheepfoldViewModel).ProgressRing = this.progress;
        }
    }
}
