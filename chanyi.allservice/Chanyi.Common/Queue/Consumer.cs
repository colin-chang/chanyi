using Chanyi.Common.Domain;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Common.Queue
{
    public class Consumer
    {

        public Consumer()
        {
        }

        public void Start<TMessageType>(TimeSpan timeOut) where TMessageType : IEvent
        {
            using (IRedisClient client = DomainHelper.GetClient())
            {
                IRedisTypedClient<TMessageType> redis = client.As<TMessageType>();

                var key = typeof(TMessageType).Name;
                var events = redis.Lists[key];

                while (true)
                {
                    TMessageType evt = redis.BlockingPopItemFromList(events, timeOut);
                    if (evt != null)
                    {
                        DomainHelper.Publish(evt);
                    }
                }
            }
        }

        public void Start<TMessageType>(string key, TimeSpan timeOut) where TMessageType : IEvent
        {
            using (IRedisClient client = DomainHelper.GetClient())
            {
                IRedisTypedClient<TMessageType> redis = client.As<TMessageType>();

                var events = redis.Lists[key];

                while (true)
                {
                    TMessageType evt = redis.BlockingPopItemFromList(events, timeOut);
                    if (evt != null)
                    {
                        DomainHelper.Publish(evt);
                    }
                }
            }
        }

    }
}
