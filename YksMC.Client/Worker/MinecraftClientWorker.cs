using Newtonsoft.Json;
using Nito.AsyncEx;
using Serilog;
using Serilog.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Client.Injection;
using YksMC.Client.Mapper;
using YksMC.Protocol;
using YksMC.Protocol.Connection;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Serializing;
using YksMC.Protocol.Models;

namespace YksMC.Client.Worker
{
    public class MinecraftClientWorker : IMinecraftClientWorker
    {
        private readonly AsyncProducerConsumerQueue<object> _sendingQueue;
        private readonly IPacketSerializer _serializer;
        private readonly IPacketBuilder _packetBuilder;
        private readonly IPacketReader _packetReader;
        private readonly IPacketDeserializer _deserializer;
        private readonly IPacketTypeMapper _typeMapper;
        private readonly MinecraftClientWorkerOptions _options;
        private readonly ILogger _logger;
        private readonly IEventDispatcher _eventDispatcher;

        private IMinecraftConnection _connection;
        private ConnectionState _state;

        public MinecraftClientWorker(IPacketSerializer serializer, IPacketBuilder packetBuilder, IPacketReader packetReader, IPacketDeserializer deserializer, IPacketTypeMapper typeMapper, MinecraftClientWorkerOptions options, ILogger logger, IEventDispatcher eventDispatcher)
        {
            _sendingQueue = new AsyncProducerConsumerQueue<object>();
            _serializer = serializer;
            _packetBuilder = packetBuilder;
            _packetReader = packetReader;
            _deserializer = deserializer;
            _typeMapper = typeMapper;
            _options = options;
            _state = ConnectionState.None;
            _logger = logger.ForContext<MinecraftClientWorker>();
            _eventDispatcher = eventDispatcher;
        }

        public void EnqueuePacket(object packet)
        {
            _sendingQueue.Enqueue(packet);
        }

        public void StartHandling(IMinecraftConnection connection)
        {
            _connection = connection;
            SetState(ConnectionState.Handshake);

            //TODO: fix
            Task[] tasks = new Task[]{
                HandleReceivingAsync(default(CancellationToken)),
                HandleSendingAsync(default(CancellationToken))
            };
        }

        private async Task HandleSendingAsync(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                object packet = await _sendingQueue.DequeueAsync(cancelToken);
                try
                {
                    await SendPacketAsync(packet, cancelToken);
                    LogPacketSent(packet);
                }
                catch (Exception ex)
                {
                    _logger.Error("{ex}", ex);
                    break;
                }
            }
        }

        private async Task SendPacketAsync(object packet, CancellationToken cancelToken)
        {
            int packetId = _typeMapper.GetPacketId(BoundTo.Server, packet.GetType());
            _packetBuilder.PutVarInt(packetId);
            _serializer.Serialize(packet, _packetBuilder);
            byte[] data = _packetBuilder.TakePacket();
            await _connection.SendPacketAsync(data, cancelToken);
        }

        private async Task HandleReceivingAsync(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                try
                {
                    await ReceiveAsync(cancelToken);
                }
                catch (Exception ex)
                {
                    _logger.Error("{ex}", ex);
                    break;
                }
            }
        }

        private async Task ReceiveAsync(CancellationToken cancelToken)
        {
            byte[] data = await _connection.ReceivePacketAsync(cancelToken);
            if (data == null)
                throw new ArgumentException("Connection closed!");

            _packetReader.SetPacket(data);
            VarInt packetId = _packetReader.GetVarInt();
            Type packetType = _typeMapper.GetPacketType(_state, BoundTo.Client, packetId.Value);
            if (packetType == null)
            {
                HandleUnsupportedPacketType(packetId.Value);
                return;
            }

            _logger.Verbose("Received packetId: 0x{id}", packetId.Value.ToString("X"));

            object packet = _deserializer.Deserialize(_packetReader, packetType);

            if (_packetReader.GetRemainingBytes() > 0)
                _logger.Verbose("Packet didn't use all the bytes! Remaining: {count}", _packetReader.GetRemainingBytes());

            LogPacketReceived(packet);
            await _eventDispatcher.DispatchEventAsync(packet);
        }

        private void LogPacketReceived(object packet)
        {
            string jsonData = JsonConvert.SerializeObject(packet);
            _logger.ForContext("packet", jsonData).Verbose("Received packet: {PacketType}", packet.GetType().Name);
        }
        private void LogPacketSent(object packet)
        {
            string jsonData = JsonConvert.SerializeObject(packet);
            _logger.ForContext("packet", jsonData).Verbose("Sent packet: {PacketType}", packet.GetType().Name);
        }

        private void HandleUnsupportedPacketType(int packetId)
        {
            string errorMessage = $"Unsupported packet type: 0x{packetId.ToString("X")}";

            if (!_options.IgnoreUnsupportedPackets)
                throw new ArgumentException(errorMessage);

            _logger.Warning(errorMessage);
        }

        public void SetState(ConnectionState state)
        {
            _state = state;
        }
    }
}
