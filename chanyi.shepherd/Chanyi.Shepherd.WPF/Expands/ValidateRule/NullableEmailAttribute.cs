using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chanyi.Shepherd.WPF.Expands.ValidateRule
{
    public class NullableEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string email = value as string;
            if (string.IsNullOrWhiteSpace(email))
                return true;

            return Regex.IsMatch(email, @"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$");
        }
    }
}
