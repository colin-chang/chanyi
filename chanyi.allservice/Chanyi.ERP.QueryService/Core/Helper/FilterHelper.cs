using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.QueryService.Core.Helper
{
    /// <summary>
    /// 查询过滤器帮助类
    /// </summary>
    public static class FilterHelper
    {
        /// <summary>
        /// 判断过滤器对象是否为Null或空对象
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns></returns>
        public static bool IsObjectEmpty(IQueryFilter filter)
        {
            if (filter == null)
                return true;
            if (!FilterHelper.HasNotEmptyElement(filter.GetType().GetProperties().Select(p => p.GetValue(filter)).ToArray()))
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
