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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.WPF.ViewModels.Inputs;

namespace Chanyi.Shepherd.WPF.Views.Inputs
{
    /// <summary>
    /// FeedInWarehouseUC.xaml 的交互逻辑
    /// </summary>
    public partial class FeedOutWarehouseUC : UserControl
    {
        public FeedOutWarehouseUC()
        {
            InitializeComponent();
        }

        public FeedOutWarehouseUC(string header, string icon, string intro)
            : this()
        {
            InOutWarehouseDirectionEnum direction = InOutWarehouseDirectionEnum.Out;
            this.DataContext = new FeedInputsViewModel(header, icon, intro,direction);
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox cb = e.Source as ComboBox;
            if (cb != null && cb.IsEditable) this.SetComboBox(cb);
        }
    }
}