using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.EventBus.Bus
{
    public interface IEventHandler<TEvent, TResult>
    {
        TResult Handle(TEvent message);        
    }
}
