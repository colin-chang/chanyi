using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Chanyi.Shepherd.WPF.Expands.Converter
{
    public abstract class Sa2VisibilityConverter : IValueConverter
    {

        string keyWord;
        public Sa2VisibilityConverter(string keyWord)
        {
            this.keyWord = keyWord;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string userName = value as string;
            return string.Equals(userName, keyWord) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class Su2VisibilityConverter : Sa2VisibilityConverter
    {
        public Su2VisibilityConverter() : base("admin") { }
    }

    public class Sr2VisibilityConverter : Sa2VisibilityConverter
    {
        public Sr2VisibilityConverter() : base("超级管理员") { }
    }
}
