using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Chanyi.Shepherd.WPF.Expands.Converter
{
    /// <summary>
    /// 2015-11  =>  11 
    /// </summary>
    class YearMonth2MonthConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DateTime.Parse(value.ToString()).ToString("yy-MM") ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw null;
        }
    }
}
