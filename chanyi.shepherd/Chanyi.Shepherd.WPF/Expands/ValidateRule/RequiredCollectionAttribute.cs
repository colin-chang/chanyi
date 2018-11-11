using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.Expands.ValidateRule
{
    public class RequiredCollectionAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var collection = value as IEnumerable;
            if (collection == null)
                return false;
            if (collection.OfType<object>().Count() <= 0)
                return false;
            return true;
        }
    }
}
