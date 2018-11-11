using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chanyi.Shepherd.WPF.Expands.ValidateRule
{
    public class FloatNumberAttribute : ValidationAttribute
    {
        public float MinValue { get; set; }

        public float MaxValue { get; set; }

        public bool IsNullable { get; set; }

        public FloatNumberAttribute(float minValue, float maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.IsNullable = false;
        }

        public override bool IsValid(object value)
        {
            var str = value as string;

            if (IsNullable && string.IsNullOrWhiteSpace(str))
                return true;
            float f;
            if (!float.TryParse(str, out f))
                return false;
            if (f < this.MinValue || f > this.MaxValue)
                return false;
            return true;
        }
    }

    public class IntNumberAttribute : ValidationAttribute
    {
        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public bool IsNullable { get; set; }

        public IntNumberAttribute(int minValue, int maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.IsNullable = false;
        }
        public override bool IsValid(object value)
        {
            var str = value as string;

            if (IsNullable && string.IsNullOrWhiteSpace(str))
                return true;
            int i;
            if (!int.TryParse(str, out i))
                return false;
            if (i < this.MinValue || i > this.MaxValue)
                return false;
            return true;
        }
    }

    public class StringToDecimalAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var str = value as string;
            decimal f;
            if (!Decimal.TryParse(str, out f))
                return false;
            if (f <= 0)
                return false;
            return true;
        }
    }
    public class StringToFloatAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var str = value as string;
            float f;
            if (!float.TryParse(str, out f))
                return false;
            if (f <= 0)
                return false;
            return true;
        }
    }
}
