﻿using Chanyi.Shepherd.WPF.ViewModels.Help;
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

namespace Chanyi.Shepherd.WPF.Views.Help
{
    /// <summary>
    /// CheckUpdateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CheckUpdateWindow : Window
    {
        public CheckUpdateWindow()
        {
            InitializeComponent();
        }

        public CheckUpdateWindow(string newVersion):this()
        {
            this.DataContext = new CheckUpdateViewModel(newVersion);
        }
    }
}
