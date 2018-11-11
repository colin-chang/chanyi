using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.Expands.ValidateRule
{
    public class BeforeTodayAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            if ((DateTime)value <= DateTime.Today)
                return true;
            return false;
        }
    }

    public class AfterTodayAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            if ((DateTime)value > DateTime.Today)
                return true;
            return false;
        }
    }
}
