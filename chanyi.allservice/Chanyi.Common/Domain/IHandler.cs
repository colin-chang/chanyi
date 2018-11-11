using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.Common.Domain
{
    public interface IHandler<T>
    {
        void Handle(T domainEvent);
    }
}
