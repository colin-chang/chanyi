using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

using Chanyi.Shepherd.QueryModel;
namespace Chanyi.Shepherd.WPF.Expands.Converter
{
    public class Gender2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GenderEnum gender = (GenderEnum)value;
            return gender == GenderEnum.Male ? "公" : "母";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class PersonGender2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GenderEnum gender = (GenderEnum)value;
            return gender == GenderEnum.Male ? "男" : "女";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class GrowthStage2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((GrowthStageEnum)value)
            {
                case GrowthStageEnum.StudSheep:
                    return "种羊";
                case GrowthStageEnum.Lamb:
                    return "羔羊";
                case GrowthStageEnum.LambHog:
                    return "育成羊";
                case GrowthStageEnum.FattingSheep:
                    return "育肥羊";
                case GrowthStageEnum.TemporaryStudSheep:
                    return "后备种羊";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class Origin2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((OriginEnum)value)
            {
                case OriginEnum.HomeBred:
                    return "自繁";
                case OriginEnum.Purchase:
                    return "购入";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SheepStatus2StringConverter : IValueConverter
    {
        public int StatusEnum { get; private set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((SheepStatusEnum)value)
            {
                case SheepStatusEnum.Nomal:
                    return "正常";
                case SheepStatusEnum.Selled:
                    return "卖出";
                case SheepStatusEnum.Dead:
                    return "死亡";
                case SheepStatusEnum.Outer:
                    return "外部";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class DeliveryWay2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((DeliveryWayEnum)value)
            {
                case DeliveryWayEnum.Normal:
                    return "顺产";
                case DeliveryWayEnum.Deliver:
                    return "助产";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class DeathDispose2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((DeathDisposeEnum)value)
            {
                case DeathDisposeEnum.Destroy:
                    return "销毁";
                case DeathDisposeEnum.Other:
                    return "其他";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class EmployeeStatus2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((EmployeeStatusEnum)value)
            {
                case EmployeeStatusEnum.OnJob:
                    return "在场";
                case EmployeeStatusEnum.Dimission:
                    return "离场";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class IsEnabled2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isEnabled = (bool)value;
            return isEnabled == true ? "是" : "否";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class InOutWarehouseStatus2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((InOutWarehouseDirectionEnum)value)
            {
                case InOutWarehouseDirectionEnum.In:
                    return "入库";
                case InOutWarehouseDirectionEnum.Out:
                    return "出库";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class OutWarehouseDispositon2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((OutWarehouseDispositonEnum)value)
            {
                case OutWarehouseDispositonEnum.SelfUse:
                    return "自用";
                case OutWarehouseDispositonEnum.Sell:
                    return "卖出";
                case OutWarehouseDispositonEnum.None:
                    return "其它";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class MidwiferyReason2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((MidwiferyReasonEnum)value)
            {
                case MidwiferyReasonEnum.Colposynizesis:
                    return "阴道狭窄";
                case MidwiferyReasonEnum.Malposition:
                    return "胎位不正";
                case MidwiferyReasonEnum.Other:
                    return "其他";
                case MidwiferyReasonEnum.None:
                    return "无";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 种羊鉴定，根据性别显示不同的内容
    /// </summary>
    public class Gender2AssessTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((GenderEnum)value)
            {
                case GenderEnum.Male:
                    return "交配能力";
                case GenderEnum.Female:
                    return "繁殖能力";
                default:
                    return "交配能力";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    //种羊编号
}
