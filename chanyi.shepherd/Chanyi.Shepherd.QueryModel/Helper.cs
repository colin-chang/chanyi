using Spring.Context;
using Spring.Context.Support;
using Spring.Data.Common;
using Spring.Data.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel
{
    public delegate List<string> GeneratorDelegate(out IDbParameters pms);

    public static class CallBuffer
    {
        public static AdoTemplate AdoTemplate
        {
            get
            {
                DbProviderFactory.DBPROVIDER_ADDITIONAL_RESOURCE_NAME = "assembly://Chanyi.Shepherd.Dao/Chanyi.Shepherd.Dao/DbProviders.xml";

                IApplicationContext context = ContextRegistry.GetContext();
                return context.GetObject<AdoTemplate>("adoTemplateProvider");
            }
        }
    }
}
