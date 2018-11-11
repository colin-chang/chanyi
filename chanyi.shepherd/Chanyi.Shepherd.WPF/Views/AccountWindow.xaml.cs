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

using Chanyi.Shepherd.IServices;
using Chanyi.Shepherd.WPF.ViewModels;
using Chanyi.Shepherd.WPF.Helper;

namespace Chanyi.Shepherd.WPF.Views
{
    /// <summary>
    /// AccountWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AccountWindow : Window
    {
        public AccountWindow()
        {
            InitializeComponent();
            this.DataContext = new AccountViewModel(this.pwd);

            if (!CompatibilityHelper.IsOSSupported())
                ckEmember.Visibility = Visibility.Hidden;
        }
    }
}
