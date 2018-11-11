using Chanyi.Common.Domain;
using log4net;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace Chanyi.Common.Queue
{
    public abstract class Producer
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Producer()
        {
        }

        protected void SendQueueMessage<TEvent>(TEvent evt) where TEvent : IEvent 
        {
            using (IRedisClient client = DomainHelper.GetClient())
            {
                IRedisTypedClient<TEvent> redis = client.As<TEvent>();

                string key = evt.GetType().Name;

                var events = redis.Lists[key];
                redis.PushItemToList(events, evt);

                Logger.InfoFormat("写入队列消息 key:{0}", key);
            }
        }

        protected void SendQueueMessage<TEvent>(TEvent evt, string key) where TEvent : IEvent
        {
            using (IRedisClient client = DomainHelper.GetClient())
            {
                IRedisTypedClient<TEvent> redis = client.As<TEvent>();

                var events = redis.Lists[key];
                redis.PushItemToList(events, evt);
                redis.Dispose();

                Logger.InfoFormat("写入队列消息 key:{0}", key);
            }
        }
    }
}
