using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.Helper
{
    public class CompatibilityHelper
    {
        /// <summary>
        /// OS内核版本是否大于或等于Window NT 6.0
        /// </summary>
        public static bool IsOSSupported()
        {
            return Environment.OSVersion.Version.Major >= 6;
        }
    }
}
