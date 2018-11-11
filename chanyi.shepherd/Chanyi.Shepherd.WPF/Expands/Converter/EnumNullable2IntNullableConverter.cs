using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

using Chanyi.Shepherd.QueryModel;

namespace Chanyi.Shepherd.WPF.Expands.Converter
{
    public class GenderNullable2IntNullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            GenderEnum gender = (GenderEnum)value;
            return ((int)gender).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (GenderEnum)int.Parse(value.ToString());
        }
    }

    public class GrowthStageNullable2IntNullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            GrowthStageEnum gender = (GrowthStageEnum)value;
            return ((int)gender).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (GrowthStageEnum)int.Parse(value.ToString());
        }
    }

    public class OriginNullable2IntNullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            OriginEnum gender = (OriginEnum)value;
            return ((int)gender).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (OriginEnum)int.Parse(value.ToString());
        }
    }

    public class SheepStatusNullable2IntNullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            SheepStatusEnum gender = (SheepStatusEnum)value;
            return ((int)gender).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (SheepStatusEnum)int.Parse(value.ToString());
        }
    }

    /// <summary>
    /// 分娩方式搜索
    /// </summary>
    public class DeliveryWayNullable2IntNullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            DeliveryWayEnum gender = (DeliveryWayEnum)value;
            return ((int)gender).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (DeliveryWayEnum)int.Parse(value.ToString());
        }
    }


    public class DeathDisposeNullable2IntNullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            DeathDisposeEnum gender = (DeathDisposeEnum)value;
            return ((int)gender).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (DeathDisposeEnum)int.Parse(value.ToString());
        }
    }


    public class EmployeeStatusNullable2IntNullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            EmployeeStatusEnum gender = (EmployeeStatusEnum)value;
            return ((int)gender).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (EmployeeStatusEnum)int.Parse(value.ToString());
        }
    }
    /// <summary>
    /// 出入库的枚举
    /// </summary>
    public class InOutWarehouseStatusNullable2IntNullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            InOutWarehouseDirectionEnum gender = (InOutWarehouseDirectionEnum)value;
            return ((int)gender).ToString();
        }

       
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (InOutWarehouseDirectionEnum)int.Parse(value.ToString());
        }
    }

    /// <summary>
    /// 饲料出库去向的枚举
    /// </summary>
    public class OutWarehouseDispositonStatusNullable2IntNullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            OutWarehouseDispositonEnum gender = (OutWarehouseDispositonEnum)value;
            return ((int)gender).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (OutWarehouseDispositonEnum)int.Parse(value.ToString());
        }
    }
}
    
