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

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.WPF.ViewModels.Inputs;

namespace Chanyi.Shepherd.WPF.Views.Inputs
{
    /// <summary>
    /// FeedInWarehouseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddFeedInOutWarehouseWindow : Window
    {
        public AddFeedInOutWarehouseWindow(InOutWarehouseDirectionEnum direction)
        {
            InitializeComponent();
            this.DataContext = new AddFeedInOutWarehouseViewModel(this.error, direction);
            //隐藏显示去向
            DirectionHeight.Height = new GridLength(0);
            this.DirectionText.Visibility = Visibility.Hidden;
            this.Direction.Visibility = Visibility.Hidden;
            if (direction == InOutWarehouseDirectionEnum.Out)
            {
                DirectionHeight.Height = new GridLength(35);
                this.DirectionText.Visibility = Visibility.Visible;
                this.Direction.Visibility = Visibility.Visible;
            }
        }
        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //设置所有可编辑ComboBox不合法内容默认选中第一项
            ComboBox cb = e.Source as ComboBox;
            if (cb != null) this.ResetComboBox(cb);
        }
    }
}
