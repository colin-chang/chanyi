using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using Chanyi.Shepherd.QueryModel.Model.Assist;
using Chanyi.Shepherd.WPF.ViewModels.Assist;

namespace Chanyi.Shepherd.WPF.Views.Assist
{
    /// <summary>
    /// MoreSymptomWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SymptomWindow : Window
    {
        public ObservableCollection<Symptom> Symptoms { get; set; }

        public SymptomWindow()
        {
            InitializeComponent();
        }
    }
}
