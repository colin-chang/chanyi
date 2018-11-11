using System.Windows;

using Chanyi.Shepherd.WPF.ViewModels.Assist;

namespace Chanyi.Shepherd.WPF.Views.Assist
{
    /// <summary>
    /// AddFormulaWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddFormulaWindow : Window
    {
        public AddFormulaWindow(string nutrientId)
        {
            InitializeComponent();
            this.DataContext = new AddFormulaViewModel(nutrientId,progress);
        }
    }
}
