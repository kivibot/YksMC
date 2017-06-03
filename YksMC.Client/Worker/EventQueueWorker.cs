using Nito.AsyncEx;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Client.Injection;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Client.Worker
{
    public class EventQueueWorker : IEventQueueWorker
    {

        private readonly AsyncProducerConsumerQueue<IPacket> _eventQueue;
        private readonly IPacketHandlerFactory _handlerFactory;
        private readonly ILogger _logger;
        public EventQueueWorker(IPacketHandlerFactory handlerFactory, ILogger logger)
        {
            _eventQueue = new AsyncProducerConsumerQueue<IPacket>();
            _handlerFactory = handlerFactory;
            _logger = logger.ForContext<EventQueueWorker>();
        }

        public void EnqueueEvent(IPacket packet)
        {
            _eventQueue.Enqueue(packet);
        }

        public void StartHandling()
        {
            //TODO: fix
            Task task = HandleReceivedPacketsAsync(default(CancellationToken));
        }

        private async Task HandleReceivedPacketsAsync(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                IPacket packet = await _eventQueue.DequeueAsync(cancelToken);
                await HandleReceivedPacketAsync((dynamic)packet);
            }
        }

        private async Task HandleReceivedPacketAsync<T>(T packet) where T : IPacket
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
