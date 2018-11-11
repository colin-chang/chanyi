using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Chanyi.Shepherd.WPF.Expands.AttcahProperty
{
    class GridColumn : DependencyObject
    {
        public static string GetBindProp(DependencyObject obj)
        {
            return (string)obj.GetValue(BindPropProperty);
        }

        public static void SetBindProp(DependencyObject obj, string value)
        {
            obj.SetValue(BindPropProperty, value);
        }

        // Using a DependencyProperty as the backing store for BindProp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindPropProperty =
            DependencyProperty.RegisterAttached("BindProp", typeof(string), typeof(GridColumn), new PropertyMetadata(string.Empty));




        public static IValueConverter GetConverter(DependencyObject obj)
        {
            return (IValueConverter)obj.GetValue(ConverterProperty);
        }

        public static void SetConverter(DependencyObject obj, IValueConverter value)
        {
            obj.SetValue(ConverterProperty, value);
        }

        // Using a DependencyProperty as the backing store for Converter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConverterProperty =
            DependencyProperty.RegisterAttached("Converter", typeof(IValueConverter), typeof(GridColumn), new PropertyMetadata(null));



    }
}
