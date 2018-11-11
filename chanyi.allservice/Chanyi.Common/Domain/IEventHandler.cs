using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.Common.Domain
{
    public interface IEventHandler
    {
        void Handle(object obj);
    }

    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent evt);
    }
}
