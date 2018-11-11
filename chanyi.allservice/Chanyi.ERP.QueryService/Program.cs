using Common.Logging;
using Spring.Context;
using Spring.Context.Support;
using Spring.ServiceModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.ERP.QueryService
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            try
            {
                // 加载配置
                IApplicationContext context = ContextRegistry.GetContext();

                // 启动多个服务。
                Logger.Info("准备启动服务。");
                Configuration conf = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
                ServiceModelSectionGroup svcmod = (ServiceModelSectionGroup)conf.GetSectionGroup("system.serviceModel");
                foreach (ServiceElement el in svcmod.Services.Services)
                {
                    if (context.ContainsObjectDefinition(el.Name))
                    {
                        Logger.Info(String.Format("启动{0}服务...", el.Name));
                        SpringServiceHost serviceHost = new SpringServiceHost(el.Name);
                        serviceHost.Opened += delegate
                        {
                            Logger.Info(el.Name + " 服务已经启动了。");
                        };
                        serviceHost.Open();
                    }
                }

                // 等待按 q 键退出。
                Logger.Info("服务已经启动，输入 q 退出！");
                string s1 = Console.ReadLine();
                while (String.Compare("q", s1, true) != 0)
                {
                    s1 = Console.ReadLine();
                }

            }
            catch (Exception ex)
            {
                Logger.Error("启动服务时发生错误", ex);
                Console.ReadLine();
            }
        }
    }
}
