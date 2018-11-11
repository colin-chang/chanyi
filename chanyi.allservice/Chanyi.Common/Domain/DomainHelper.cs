using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.Common.Domain
{
    public static class DomainHelper
    {
        public static void Publish(IEvent evt)
        {
            DomainConfiguration.Instance.Bus.Publish(evt);
        }

        public static IRedisClient GetClient()
        {
            return DomainConfiguration.Instance.RedisClientManager.GetClient();
        }
    }
}
