using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chanyi.Shepherd.IServices;
using Chanyi.Shepherd.WPF.Expands.Helper;

namespace System.Windows
{
    /// <summary>
    /// Window扩展方法
    /// </summary>
    public static class WindowExpand
    {
        /// <summary>
        /// 获取IService服务对象
        /// </summary>
        /// <param name="cx"></param>
        /// <returns></returns>
        public static IService GetService(this Window cx)
        {
            return CommonExpand.GetService();
        }
    }
}
