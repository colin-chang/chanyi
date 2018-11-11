using System.Windows;
using System.Windows.Controls;

using Chanyi.Shepherd.WPF.ViewModels.BaseInfo;

namespace Chanyi.Shepherd.WPF.Views.BaseInfo
{
    /// <summary>
    /// EntrySheepWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddSheepWindow : Window
    {
        public AddSheepWindow()
        {
            InitializeComponent();
            this.DataContext = new AddSheepViewModel(this.error);
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //设置所有可编辑ComboBox不合法内容默认选中第一项
            ComboBox cb = e.Source as ComboBox;
            if (cb == null)
                return;
            if (cb.Tag != null && cb.Tag.ToString().StartsWith("NoValidate"))
            {
                this.SetComboBox(cb);

                var vm = this.DataContext as AddSheepViewModel;
                if (vm == null)
                    return;
                if (cb.Tag.ToString().EndsWith("F"))
                    vm.FatherSerialNumber = cb.Text;
                else
                    vm.MotherSerialNumber = cb.Text;
            }
            else
                this.ResetComboBox(cb);
        }

        private void OrginChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != 0)
                HideBuyForm();
            else
                ShowBuyForm();
        }

        public void ShowBuyForm()
        {
            this.Height = 665;
            this.buyForm.Height = new GridLength(225);
        }

        void HideBuyForm()
        {
            this.Height = 420;
            this.buyForm.Height = new GridLength(0);
        }
    }
}
