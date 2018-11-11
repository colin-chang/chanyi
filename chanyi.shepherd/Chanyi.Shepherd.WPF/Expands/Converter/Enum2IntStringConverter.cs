using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

using Chanyi.Shepherd.QueryModel;

namespace Chanyi.Shepherd.WPF.Expands.Converter
{
    public class GrowthStage2IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)(GrowthStageEnum)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (GrowthStageEnum)int.Parse(value.ToString());
        }
    }

    public class Origin2IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)(OriginEnum)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (OriginEnum)int.Parse(value.ToString());
        }
    }

    public class Status2IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)(EmployeeStatusEnum)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (EmployeeStatusEnum)int.Parse(value.ToString());
        }
    }
    public class DeliveryWay2IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)(DeliveryWayEnum)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (DeliveryWayEnum)int.Parse(value.ToString());
        }
    }

    public class DeathDispose2IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)(DeathDisposeEnum)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (DeathDisposeEnum)int.Parse(value.ToString());
        }
    }

    public class MidwiferyReason2IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)(MidwiferyReasonEnum)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (MidwiferyReasonEnum)int.Parse(value.ToString());
        }
    }
}
