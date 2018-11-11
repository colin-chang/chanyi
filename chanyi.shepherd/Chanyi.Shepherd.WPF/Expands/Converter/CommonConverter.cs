using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

using Chanyi.Shepherd.WPF.Model;
using Chanyi.Shepherd.QueryModel.Model.System;
using Chanyi.Shepherd.QueryModel.Model.Formula;


namespace Chanyi.Shepherd.WPF.Expands.Converter
{
    public class BoolReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    public class Bool2YesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (bool)value;
            return b ? "是" : "否";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
            //throw new NotImplementedException();
        }
    }

    public class Bool2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Visibility)value) == Visibility.Visible ? true : false;
        }
    }

    public class Bool2VisiblityReverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Visibility)value) == Visibility.Visible ? false : true;
        }
    }

    public class HasValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? "-" : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class NotSelfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string currentId = value as string;
            string userId = Application.Current.Properties["Uid"] as string;
            return !string.Equals(currentId, userId);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class HasPermissionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string url = value as string;
            if (string.IsNullOrWhiteSpace(url))
                return false;

            var perms = Application.Current.Properties["Permissions"] as List<Permission>;
            if (perms == null || perms.Count() <= 0)
                return false;

            return perms.Where(p => p.URL == url).Count() > 0;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }


    public class HasPermissionEditableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null)
                return false;

            //string obj1 = values[0] as string;
            string obj1 = values[0].ToString();
            if (obj1 == null)
                return false;
            bool isEditable;
            if (!bool.TryParse(obj1, out isEditable))
                return false;
            if (!isEditable)
                return false;

            HasPermissionConverter cvt = new HasPermissionConverter();
            return cvt.Convert(values[1], targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[0];
        }
    }

    /// <summary>
    /// 行内的编辑
    /// </summary>
    public class HasRowPermissionEditableConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertObject2Bool(values[0]) && ConvertObject2Bool(values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[0];
        }

        private bool ConvertObject2Bool(object obj)
        {
            if (obj == null)
                return false;

            string obj1 = obj.ToString();
            if (obj1 == null)
                return false;
            bool isEditable;
            if (!bool.TryParse(obj1, out isEditable))
                return false;
            return isEditable;
        }
    }


}
