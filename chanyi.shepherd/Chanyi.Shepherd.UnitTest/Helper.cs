using Chanyi.Shepherd.IServices;
using Spring.Context;
using Spring.Context.Support;
using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.UnitTest
{
    public class Helper
    {
        public static IService GetService()
        {
            DbProviderFactory.DBPROVIDER_ADDITIONAL_RESOURCE_NAME = "assembly://Chanyi.Shepherd.Dao/Chanyi.Shepherd.Dao/DbProviders.xml";

            IApplicationContext context = ContextRegistry.GetContext();
            return context.GetObject<IService>("services");
        }

        public static IService Service { get { return GetService(); } }

    }
}
