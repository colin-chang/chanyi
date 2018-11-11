using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.Utility.Common
{
    public static class StringHelper
    {
        /// <summary>
        /// 判断字符串集合中是否存在任何空元素
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取字符串类型数字的值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float? ToFloat(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            float n;
            return float.TryParse(str, out n) ? (float?)n : null;
        }

        /// <summary>
        /// 判断是否能转换成decimal类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool TryDecimal(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            decimal a;
            return decimal.TryParse(str, out a);
        }

        public static bool TryFloat(this string str)
        {
            float a;
            return float.TryParse(str, out a);
        }
    }
}
