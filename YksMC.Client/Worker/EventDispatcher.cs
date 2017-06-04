using Nito.AsyncEx;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Client.Injection;
using YksMC.Protocol.Models;

namespace YksMC.Client.Worker
{
    public class EventDispatcher : IEventDispatcher
    {        
        private readonly IPacketHandlerFactory _handlerFactory;
        private readonly ILogger _logger;

        public EventDispatcher(IPacketHandlerFactory handlerFactory, ILogger logger)
        {
            _handlerFactory = handlerFactory;
            _logger = logger.ForContext<EventDispatcher>();
        }

        public async Task DispatchEventAsync(object packet)
        {
            await HandleReceivedPacketAsync((dynamic)packet);
        }

        private async Task HandleReceivedPacketAsync<T>(T packet)
        {
            using (IOwned<IEnumerable<IPacketHandler<T>>> handlers = _handlerFactory.GetHandlers<T>())
            {
                foreach (IPacketHandler<T> handler in handlers.Value)
                {
                    try
                    {
                        await handler.HandleAsync(packet);
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
