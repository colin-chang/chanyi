using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.Common.Domain
{
    public class EventsBus
    {
        //private readonly List<IDomainEvent> events = new List<IDomainEvent>();
        private readonly IDictionary<Type, IList<IEventHandler>> handlerDic = new Dictionary<Type, IList<IEventHandler>>();


        internal EventsBus() { }


        public void Start()
        {

        }

        public void Publish(IEvent evt)
        {
            if (evt == null)
                throw new ArgumentNullException("不能发布 null 领域事件。");

            var eventType = evt.GetType();

            IList<IEventHandler> eventHandlers;
            handlerDic.TryGetValue(eventType, out eventHandlers);

            if (eventHandlers == null || eventHandlers.Count == 0)
                throw new NotSupportedException(String.Format("领域事件({0})没有相应的事件处理器，无法处理", eventType.Name));

            foreach (IEventHandler eventHandler in eventHandlers)
            {
                eventHandler.Handle(evt);
            }
        }

        public EventsBus RegisterEventHandler(Object obj)
        {
            if (obj == null)
                return this;

            IEnumerable<Type> genericsInterfaceTypes = obj.GetType().GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEventHandler<>));
            foreach (Type genericsInterfaceType in genericsInterfaceTypes)
            {
                Type eventType = genericsInterfaceType.GetGenericArguments().Single();

                IList<IEventHandler> eventHandlers;
                handlerDic.TryGetValue(eventType, out eventHandlers);
                if (eventHandlers == null)
                {
                    eventHandlers = new List<IEventHandler>();
                    handlerDic.Add(eventType, eventHandlers);
                }

                var eventHandlerWrapperType = typeof(EventHandlerWrapper<>).MakeGenericType(eventType);
                var eventHandler = Activator.CreateInstance(eventHandlerWrapperType, new[] { obj }) as IEventHandler;
                eventHandlers.Add(eventHandler);
            }

            return this;
        }


    }
}
