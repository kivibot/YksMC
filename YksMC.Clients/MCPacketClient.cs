using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol;
using YksMC.Protocol.Models.Packets;
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

        private IMCConnection _connection;
        private bool _isConnected;

        public MCPacketClient(TcpClient tcpClient, IMCPacketReader reader, IMCPacketBuilder builder, IMCPacketDeserializer deserializer, IMCPacketSerializer serializer, StreamMCConnection.Factory connectionFactory)
        {
            _tcpClient = tcpClient;
            _reader = reader;
            _builder = builder;
            _deserializer = deserializer;
            _serializer = serializer;
            _connectionFactory = connectionFactory;
        }

        public async Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken))
        {
            await _tcpClient.ConnectAsync(host, port);
            _connection = _connectionFactory(_tcpClient.GetStream());
            _isConnected = true;
        }

        public async Task<T> ReceiveAsync<T>(CancellationToken cancelToken = default(CancellationToken)) where T : IPacket
        {
            CheckConnectionState();

            byte[] data = await _connection.ReceivePacketAsync(cancelToken);
            if (data == null)
                throw new InvalidOperationException("Connection closed.");
            _reader.SetPacket(data);
            return _deserializer.Deserialize<T>(_reader);
        }

        public async Task SendAsync(IPacket packet, CancellationToken cancelToken = default(CancellationToken))
        {
            CheckConnectionState();

            _serializer.Serialize(packet, _builder);
            byte[] data = _builder.TakePacket();
            await _connection.SendPacketAsync(data, cancelToken);
        }

        private void CheckConnectionState()
        {
            if (!_isConnected)
                throw new InvalidOperationException("Not connected!");
        }
    }
}
