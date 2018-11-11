using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter
{
    /// <summary>
    /// 查询过滤器帮助类
    /// </summary>
    public static class FilterHelper
    {
        /// <summary>
        /// 判断过滤器对象是否为Null或空对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public static bool IsObjectEmpty<T>(T obj) where T : class
        //{
        //    if (obj == null)
        //        return true;

        //    if (!FilterHelper.HasNotEmptyElement(obj.GetType().GetProperties().Select(p => p.GetValue(obj,null)).ToArray()))
        //        return true;

        //    return false;
        //}
        public static bool IsObjectEmpty(IQueryFilter obj)
        {
            if (obj == null)
                return true;

            if (!FilterHelper.HasNotEmptyElement(obj.GetType().GetProperties().Select(p => p.GetValue(obj, null)).ToArray()))
                return true;

            return false;
        }

  

        /// <summary>
        /// 判断过滤器参数是否有非空元素
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static bool HasNotEmptyElement(params object[] elements)
        {
            foreach (object e in elements)
            {
                if (e is ValueType)
                    return true;
                if (e is string && !string.IsNullOrWhiteSpace((e as string)))
                    return true;
                if (e != null)
                    return true;
            }
            return false;
        }
    }
}
