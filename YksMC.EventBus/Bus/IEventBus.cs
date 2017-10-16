using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YksMC.EventBus.Bus
{
    public interface IEventBus
    {
        IReadOnlyList<TResult> Handle<TEvent, TResult>(TEvent message);
    }
}
