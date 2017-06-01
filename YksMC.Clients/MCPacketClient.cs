using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Clients.Mapper;
using YksMC.Protocol;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Serializing;

namespace YksMC.Clients
{
    public class MCPacketClient : IMCPacketClient
    {
        private readonly TcpClient _tcpClient;
        private readonly IMCPacketReader _reader;
        private readonly IMCPacketBuilder _builder;
        private readonly IMCPacketDeserializer _deserializer;
        private readonly IMCPacketSerializer _serializer;
        private readonly StreamMCConnection.Factory _connectionFactory;
        private readonly IPacketTypeMapper _typeMapper;

        private IMCConnection _connection;
        private bool _isConnected;
        private ConnectionState _state;

        public MCPacketClient(TcpClient tcpClient, IMCPacketReader reader, IMCPacketBuilder builder, IMCPacketDeserializer deserializer, IMCPacketSerializer serializer, StreamMCConnection.Factory connectionFactory, IPacketTypeMapper typeMapper)
        {
            _tcpClient = tcpClient;
            _reader = reader;
            _builder = builder;
            _deserializer = deserializer;
            _serializer = serializer;
            _connectionFactory = connectionFactory;
            _typeMapper = typeMapper;
        }

        public async Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken))
        {
            await _tcpClient.ConnectAsync(host, port);
            _connection = _connectionFactory(_tcpClient.GetStream());
            _isConnected = true;
        }

        public async Task<T> ReceiveAsync<T>(CancellationToken cancelToken = default(CancellationToken)) where T : IPacket
        {
            return (T)await ReceiveAsync(cancelToken);
        }

        public async Task<IPacket> ReceiveAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            CheckConnectionState();

            byte[] data = await _connection.ReceivePacketAsync(cancelToken);
            if (data == null)
                throw new InvalidOperationException("Connection closed.");
            _reader.SetPacket(data);

            VarInt packetId = _reader.GetVarInt();
            _reader.ResetPosition();
            Type packetType = _typeMapper.GetPacketType(_state, BoundTo.Client, packetId.Value);
            if (packetType == null)
                throw new ArgumentException($"No packet registered for: [{_state}, {BoundTo.Client}, {packetId}]");
            return (IPacket)_deserializer.Deserialize(_reader, packetType);
        }

        public async Task SendAsync(IPacket packet, CancellationToken cancelToken = default(CancellationToken))
        {
            CheckConnectionState();

            _serializer.Serialize(packet, _builder);
            byte[] data = _builder.TakePacket();
            await _connection.SendPacketAsync(data, cancelToken);
        }

        public void SetState(ConnectionState state)
        {
            _state = state;
        }

        private void CheckConnectionState()
        {
            if (!_isConnected)
                throw new InvalidOperationException("Not connected!");
        }
    }
}
