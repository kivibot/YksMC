using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YksMC.EventBus.Bus
{
    public interface IEventBus
    {
        IReadOnlyList<TResult> Handle<TEvent, TResult>(TEvent message);
        TResult HandleAsPipeline<TEvent, TResult>(TEvent message, Func<TResult, TEvent> resultTransformer);
    }
}
