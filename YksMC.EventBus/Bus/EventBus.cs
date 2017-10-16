using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YksMC.EventBus.Bus
{
    public class EventBus : IEventBus
    {
        private readonly IHandlerContainer _handlerContainer;

        public EventBus(IHandlerContainer handlerContainer)
        {
            _handlerContainer = handlerContainer;
        }

        public IReadOnlyList<TResult> Handle<TEvent, TResult>(TEvent message)
        {
            List<TResult> results = new List<TResult>();

            IReadOnlyList<IEventHandler<TEvent, TResult>> handlers = _handlerContainer.GetHandlers<TEvent, TResult>();
            try
            {
                foreach (IEventHandler<TEvent, TResult> handler in handlers)
                {
                    TResult result = ActivateHandler(message, handler);
                    results.Add(result);
                }
            }
            finally
            {
                ReturnHandlers(handlers);
            }

            return results;
        }

        private TResult ActivateHandler<TEvent, TResult>(TEvent message, IEventHandler<TEvent, TResult> handler)
        {
            TResult result = handler.Handle(message);
            return result;
        }

        private void ReturnHandlers<TEvent, TResult>(IReadOnlyList<IEventHandler<TEvent, TResult>> handlers)
        {
            foreach (IEventHandler<TEvent, TResult> handler in handlers)
            {
                _handlerContainer.ReturnHandler(handler);
            }
        }
    }
}
