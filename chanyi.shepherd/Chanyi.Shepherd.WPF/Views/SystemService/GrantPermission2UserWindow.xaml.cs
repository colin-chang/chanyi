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

namespace Chanyi.Shepherd.WPF.Views.SystemService
{
    /// <summary>
    /// AddCustomFormulaWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GrantPermission2UserWindow : Window
    {

        public GrantPermission2UserWindow()
        {
            InitializeComponent();
        }

        public GrantPermission2UserWindow(string id)
            : this()
        {
            this.DataContext = new GrantPermission2UserViewModel(id, isFailed => this.GrantFailed = isFailed);
        }

        /// <summary>
        /// 分配权限失败
        /// </summary>
        public bool GrantFailed { get; set; }
    }
}
