using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.Common.Domain
{
    public class EventHandlerWrapper<TEvent> : IEventHandler where TEvent : IEvent
    {
        private IEventHandler<TEvent> eventHandler;

        public EventHandlerWrapper(IEventHandler<TEvent> domainEventHandler)
        {
            this.eventHandler = domainEventHandler;
        }

        public void Handle(object obj)
        {
            this.eventHandler.Handle((TEvent)obj);
        }
    }
}
