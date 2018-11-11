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

using Chanyi.Shepherd.WPF.ViewModels.SystemService;
using Chanyi.Shepherd.QueryModel.Model.HR;

namespace Chanyi.Shepherd.WPF.Views.SystemService
{
    /// <summary>
    /// UserUC.xaml 的交互逻辑
    /// </summary>
    public partial class UserUC : UserControl
    {
        public UserUC()
        {
            InitializeComponent();
        }

        public UserUC(string header, string icon, string intro)
            : this()
        {
            this.DataContext = new UserViewModel(header, icon, intro, this.lkEdit.Tag as string);
        }
    }
}
