﻿using Chanyi.Shepherd.WPF.ViewModels.Breeding;
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

namespace Chanyi.Shepherd.WPF.Views.Breeding
{
    /// <summary>
    /// SecondAssessWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddSecondAssessWindow : Window
    {
        public AddSecondAssessWindow()
        {
            InitializeComponent();
            this.DataContext = new AddSecondAssessViewModel(this.error);
        }
        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //设置所有可编辑ComboBox不合法内容默认选中第一项
            ComboBox cb = e.Source as ComboBox;
            if (cb != null) this.ResetComboBox(cb);
        }
    }
}
