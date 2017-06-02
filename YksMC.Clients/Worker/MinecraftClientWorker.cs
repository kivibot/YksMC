using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Clients.Mapper;
using YksMC.Protocol;
using YksMC.Protocol.Connection;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Serializing;

namespace YksMC.Clients.Worker
{
    public class MinecraftClientWorker : IMinecraftClientWorker
    {
        private readonly AsyncProducerConsumerQueue<IPacket> _outgoingQueue;
        private readonly IPacketSerializer _serializer;
        private readonly IPacketBuilder _packetBuilder;
        private readonly IPacketReader _packetReader;
        private readonly IPacketDeserializer _deserializer;
        private readonly IPacketTypeMapper _typeMapper;

        private IMinecraftConnection _connection;
        private ConnectionState _state;

        public MinecraftClientWorker(IPacketSerializer serializer, IPacketBuilder packetBuilder, IPacketReader packetReader, IPacketDeserializer deserializer, IPacketTypeMapper typeMapper)
        {
            _outgoingQueue = new AsyncProducerConsumerQueue<IPacket>();
            _serializer = serializer;
            _packetBuilder = packetBuilder;
            _packetReader = packetReader;
            _deserializer = deserializer;
            _typeMapper = typeMapper;
        }

        public void EnqueuePacket(IPacket packet)
        {
            _outgoingQueue.Enqueue(packet);
        }

        public void StartHandling(IMinecraftConnection connection)
        {
            _connection = connection;
            _state = ConnectionState.Handshake;

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
                IPacket packet = await _outgoingQueue.DequeueAsync(cancelToken);
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
            Type packetType = _typeMapper.GetPacketType(ConnectionState.Login, BoundTo.Client, packetId.Value);
            if (packetType == null)
                throw new ArgumentException("Unsupported packet type: " + packetId.Value.ToString("X"));
            _packetReader.ResetPosition();
            IPacket packet = (IPacket)_deserializer.Deserialize(_packetReader, packetType);
        }

        public void SetState(ConnectionState state)
        {
            _state = state;
        }
    }
}
