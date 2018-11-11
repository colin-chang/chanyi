using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Spring.Context;
using Spring.Context.Support;
using Spring.Data.Common;

using Chanyi.Shepherd.IServices;
using Chanyi.Shepherd.WPF.Expands.Helper;

namespace Microsoft.Practices.Prism.ViewModel
{
    /// <summary>
    /// ViewModel扩展方法
    /// </summary>
    public static class NotificationObjectExpand
    {
        /// <summary>
        /// 获取IService服务对象
        /// </summary>
        /// <param name="cx"></param>
        /// <returns></returns>
        public static IService GetService(this NotificationObject cx)
        {
            return CommonExpand.GetService();
        }
    }
}
