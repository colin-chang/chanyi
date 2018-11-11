using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.Utility.Common
{
    public static class StringHelper
    {
        public static bool HasAnyEmptyElement(params string[] collection)
        {
            if (collection == null || collection.Length <= 0)
                return true;

            foreach (string str in collection)
                if (string.IsNullOrWhiteSpace(str))
                    return true;

            return false;
        }

        /// <summary>
        /// 将字符串以给定符号包装 如："s" -> "%s%"
        /// </summary>
        /// <param name="str"></param>
        /// <param name="symbol">包装符号</param>
        /// <returns></returns>
        public static string Wrap(this string str, string symbol)
        {
            return symbol + str + symbol;
        }
    }
}
