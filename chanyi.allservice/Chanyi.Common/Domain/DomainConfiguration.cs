using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.Common.Domain
{
    public class DomainConfiguration
    {
        private static DomainConfiguration instance;
        private static readonly object syncObj = new object();

        private PooledRedisClientManager redisClientManager;
        private EventsBus eventBus;

        private DomainConfiguration()
        {
            this.eventBus = new EventsBus();
        }

        private DomainConfiguration(string redisServer)
        {
            this.eventBus = new EventsBus();
            redisClientManager = new PooledRedisClientManager(redisServer);
        }

        public static DomainConfiguration Create()
        {
            if (instance == null)
            {
                lock (syncObj)
                {
                    if (instance == null)
                    {
                        instance = new DomainConfiguration();
                    }
                }
            }
            return instance;
        }

        public static DomainConfiguration Create(string redisServer)
        {
            if (instance == null)
            {
                lock (syncObj)
                {
                    if (instance == null)
                    {
                        instance = new DomainConfiguration(redisServer);
                    }
                }
            }
            return instance;
        }

        public static DomainConfiguration Instance
        {
            get
            {
                return instance;
            }
        }

        public DomainConfiguration RegisterEventHandler(object obj)
        {
            if (obj != null)
                this.eventBus.RegisterEventHandler(obj);

            return this;
        }

        public EventsBus Bus
        {
            get
            {
                return eventBus;
            }
        }

        public PooledRedisClientManager RedisClientManager
        {
            get
            {
                return redisClientManager;
            }
        }
    }
}
