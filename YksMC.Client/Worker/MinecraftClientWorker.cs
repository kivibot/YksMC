﻿using Nito.AsyncEx;
using Serilog;
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
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Serializing;

namespace YksMC.Client.Worker
{
    public class MinecraftClientWorker : IMinecraftClientWorker
    {
        private readonly AsyncProducerConsumerQueue<IPacket> _sendingQueue;
        private readonly IPacketSerializer _serializer;
        private readonly IPacketBuilder _packetBuilder;
        private readonly IPacketReader _packetReader;
        private readonly IPacketDeserializer _deserializer;
        private readonly IPacketTypeMapper _typeMapper;
        private readonly MinecraftClientWorkerOptions _options;
        private readonly ILogger _logger;
        private readonly IEventQueueWorker _eventQueue;

        private IMinecraftConnection _connection;
        private ConnectionState _state;

        public MinecraftClientWorker(IPacketSerializer serializer, IPacketBuilder packetBuilder, IPacketReader packetReader, IPacketDeserializer deserializer, IPacketTypeMapper typeMapper, MinecraftClientWorkerOptions options, ILogger logger, IEventQueueWorker eventQueue)
        {
            _sendingQueue = new AsyncProducerConsumerQueue<IPacket>();
            _serializer = serializer;
            _packetBuilder = packetBuilder;
            _packetReader = packetReader;
            _deserializer = deserializer;
            _typeMapper = typeMapper;
            _options = options;
            _state = ConnectionState.None;
            _logger = logger;
            _eventQueue = eventQueue;
        }

        public void EnqueuePacket(IPacket packet)
        {
            _sendingQueue.Enqueue(packet);
        }

        public void StartHandling(IMinecraftConnection connection)
        {
            _connection = connection;
            _eventQueue.StartHandling();
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
                IPacket packet = await _sendingQueue.DequeueAsync(cancelToken);
                await SendPacketAsync(packet, cancelToken);
            }
        }

        private async Task SendPacketAsync(IPacket packet, CancellationToken cancelToken)
        {
            _serializer.Serialize(packet, _packetBuilder);
            byte[] data = _packetBuilder.TakePacket();
            await _connection.SendPacketAsync(data, cancelToken);
        }

        private async Task HandleReceivingAsync(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                await ReceiveAsync(cancelToken);
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
                HandleUnsupportedPacketType(packetId.Value);

            _packetReader.ResetPosition();
            IPacket packet = (IPacket)_deserializer.Deserialize(_packetReader, packetType);

            _logger.Verbose("Received packet: {type}", packetType.Name);
            _eventQueue.EnqueueEvent(packet);
        }

        private void HandleUnsupportedPacketType(int packetId)
        {
            string errorMessage = $"Unsupported packet type: 0x{packetId.ToString("X")}";

            if (!_options.IgnoreUnsupportedPackets)
                throw new ArgumentException(errorMessage);

            _logger.Debug(errorMessage);
        }

        public void SetState(ConnectionState state)
        {
            _state = state;
        }
    }
}
