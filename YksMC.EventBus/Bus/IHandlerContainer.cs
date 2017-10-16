using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.EventBus.Bus
{
    public interface IHandlerContainer
    {
        IReadOnlyList<IEventHandler<TEvent, TResult>> GetHandlers<TEvent, TResult>();
        void ReturnHandler<TEvent, TResult>(IEventHandler<TEvent, TResult> handler);
    }
}
