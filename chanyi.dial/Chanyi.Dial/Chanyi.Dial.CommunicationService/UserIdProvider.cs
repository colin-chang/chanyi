using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.Dial.CommunicationService
{
    /// <summary>
    /// 用户名获取方式提供者
    /// </summary>
    public class CookieUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            if (request == null)
                throw new ArgumentException("request对象为null");
            Cookie cookie;
            if (request.Cookies.TryGetValue(ConfigurationManager.AppSettings["UserId"], out cookie))
                return cookie.Value;
            else
                return null;
        }
    }
}
