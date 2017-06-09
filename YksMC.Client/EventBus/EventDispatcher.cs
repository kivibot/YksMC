using Nito.AsyncEx;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Client.Handler;
using YksMC.Client.Injection;
using YksMC.Protocol.Models;

namespace YksMC.Client.EventBus
{
    public class EventDispatcher : IEventBus
    {        
        private readonly IPacketHandlerFactory _handlerFactory;
        private readonly ILogger _logger;

        public EventDispatcher(IPacketHandlerFactory handlerFactory, ILogger logger)
        {
            _handlerFactory = handlerFactory;
            _logger = logger.ForContext<EventDispatcher>();
        }

        public void DispatchEvent(object eventArgs)
        {
            HandleEvent((dynamic)eventArgs);
        }

        private void HandleEvent<T>(T eventArgs)
        {
            using (IOwned<IEnumerable<IEventHandler<T>>> handlers = _handlerFactory.GetHandlers<T>())
            {
                foreach (IEventHandler<T> handler in handlers.Value)
                {
                    try
                    {
                        handler.Handle(eventArgs);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("{ex}", ex);
                    }
                }
            }
        }
    }
}
