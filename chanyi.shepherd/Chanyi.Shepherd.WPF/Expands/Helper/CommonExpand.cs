using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Spring.Context;
using Spring.Context.Support;
using Spring.Data.Common;

using Chanyi.Shepherd.IServices;

namespace Chanyi.Shepherd.WPF.Expands.Helper
{
    public static class CommonExpand
    {
        /// <summary>
        /// 获取IService服务对象
        /// </summary>
        /// <returns></returns>
        public static IService GetService()
        {
            string name = typeof(CommonExpand).FullName;
            IService service = Application.Current.Properties[name] as IService;
            if (service == null)
            {
                DbProviderFactory.DBPROVIDER_ADDITIONAL_RESOURCE_NAME = "assembly://Chanyi.Shepherd.Dao/Chanyi.Shepherd.Dao/DbProviders.xml";
                IApplicationContext context = ContextRegistry.GetContext();
                service = context.GetObject<IService>("services");
                Application.Current.Properties[name] = service;
            }
            return service;
        }
    }
}
