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

using Chanyi.Shepherd.WPF.ViewModels.SystemService;

namespace Chanyi.Shepherd.WPF.Views.SystemService
{
    /// <summary>
    /// EditUserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditPermissionWindow : Window
    {
        public EditPermissionWindow()
        {
            InitializeComponent();
        }

        public EditPermissionWindow(string id)
            : this()
        {
            this.DataContext = new EditPermissionViewModel(id);
        }
    }
}
