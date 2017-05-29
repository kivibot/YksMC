using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Clients.Models.Dtos;
using YksMC.Protocol;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Serializing;

namespace YksMC.Clients
{
    public class MCStatusClient : IMCStatusClient, IDisposable
    {
        private IMCPacketClient _client;
        private string _host;
        private ushort _port;
        private bool _disposed;
        private TcpClient _tcpClient;

        public MCStatusClient()
        {
            _tcpClient = new TcpClient();
        }

        public async Task<ServerStatus> GetStatusAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            CheckState();

            await _client.SendAsync(new HandshakePacket()
            {
                ProtocolVersion = ProtocolVersion.Unknown,
                ServerAddress = _host,
                ServerPort = _port,
                NextState = SubProtocol.Status
            }, cancelToken);
            await _client.SendAsync(new StatusRequestPacket(), cancelToken);

            StatusResponsePacket responsePacket = await _client.ReceiveAsync<StatusResponsePacket>(cancelToken);

            await _client.SendAsync(new PingPacket()
            {
                Payload = DateTimeOffset.UtcNow.Ticks
            }, cancelToken);

            PongPacket pongPacket = await _client.ReceiveAsync<PongPacket>(cancelToken);
            TimeSpan ping = TimeSpan.FromTicks(DateTimeOffset.UtcNow.Ticks - pongPacket.Payload);

            ServerStatus status = JsonConvert.DeserializeObject<ServerStatus>(responsePacket.JsonData);
            status.Ping = ping;

            Dispose();

            return status;
        }

        public async Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken))
        {
            _host = host;
            _port = port;
            await _tcpClient.ConnectAsync(host, port);
            StreamMCConnection connection = new StreamMCConnection(_tcpClient.GetStream());
            _client = new MCPacketClient(new MCPacketReader(connection), new MCPacketWriter(connection), new MCPacketDeserializer(), new MCPacketSerializer());
        }

        public void Dispose()
        {
            _tcpClient.Dispose();
            _disposed = true;
        }

        private void CheckState()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MCStatusClient));
            if (_client == null)
                throw new InvalidOperationException("Not connected!");
        }
    }
}
