﻿using System;
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

using Chanyi.Shepherd.WPF.ViewModels.Help;

namespace Chanyi.Shepherd.WPF.Views.Help
{
    /// <summary>
    /// FeedbackWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FeedbackWindow : Window
    {
        public FeedbackWindow()
        {
            InitializeComponent();
            this.DataContext = new FeedbackViewModel();
        }
    }
}
