using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(Chanyi.Dial.CommunicationService.Startup))]

namespace Chanyi.Dial.CommunicationService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //注册用户名提供者
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new CookieUserIdProvider());
            app.Map("/signalr", map =>
            {
                //允许跨域访问
                map.UseCors(CorsOptions.AllowAll);
                var config = new HubConfiguration
                {
                    //激活JSONP兼容老浏览器
                    EnableJSONP = true,
                    //允许动态生成JS代理
                    EnableJavaScriptProxies = true,
                    //允许发送详细错误消息
#if DEBUG
                    EnableDetailedErrors = true
#endif
                };
                map.RunSignalR(config);
            });
        }
    }
}
