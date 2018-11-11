using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

using Chanyi.Shepherd.QueryModel;
namespace Chanyi.Shepherd.WPF.Expands.Converter
{
    public class Gender2BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GenderEnum gender = (GenderEnum)value;
            return gender == GenderEnum.Male;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isMale = (bool)value;
            return isMale ? GenderEnum.Male : GenderEnum.Female;
        }
    }
}
