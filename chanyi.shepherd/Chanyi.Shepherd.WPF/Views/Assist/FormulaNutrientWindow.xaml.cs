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
    /// FormulaNutrientWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FormulaNutrientWindow : Window
    {
        public FormulaNutrientWindow()
        {
            InitializeComponent();
            this.gridContent.Children.OfType<UIElement>().Where(u => u is TextBlock).Select(u => u as TextBlock).ToList().ForEach(t => t.ToolTip = t.Text.TrimEnd('：'));
        }

        public FormulaNutrientWindow(bool isEdit) : this(isEdit, null)
        {
        }

        public FormulaNutrientWindow(bool isEdit, string id) : this()
        {
            this.DataContext = isEdit ? (object)new EditFormulaNutrientViewModel(id) : new AddFormulaNutrientViewModel(this.error);
        }
    }
}
