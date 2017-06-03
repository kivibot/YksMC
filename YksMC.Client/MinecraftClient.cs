using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Client.Models.Status;
using YksMC.Client.Worker;
using YksMC.Protocol;
using YksMC.Protocol.Connection;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Packets.Login;
using YksMC.Protocol.Models.Packets.Status;
using YksMC.Protocol.Serializing;
using YksMC.Client.Models;
using Serilog;

namespace YksMC.Client
{
    public class MinecraftClient : IMinecraftClient
    {
        private const ProtocolVersion _protocolVersion = ProtocolVersion.v1_11_2;

        private readonly TcpClient _tcpClient;
        private readonly IMinecraftClientWorker _worker;
        private readonly StreamMinecraftConnection.Factory _connectionFactory;
        private readonly ILogger _logger;

        private IMinecraftConnection _connection;
        private ServerAddress _address;

        public ProtocolVersion ProtocolVersion => _protocolVersion;
        public ServerAddress Address => _address;

        public MinecraftClient(TcpClient tcpClient, IMinecraftClientWorker worker, StreamMinecraftConnection.Factory connectionFactory, ILogger logger)
        {
            _tcpClient = tcpClient;
            _worker = worker;
            _connectionFactory = connectionFactory;
            _logger = logger.ForContext<MinecraftClient>();
        }

        public async Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken))
        {
            EnsureNotConnected();
            _logger.Information("Connection to: tcp://{host}:{port}", host, port);

            await _tcpClient.ConnectAsync(host, port);
            _connection = _connectionFactory(_tcpClient.GetStream());
            _worker.StartHandling(_connection);
            _address = new ServerAddress(host, port);

            SetState(ConnectionState.Handshake);
        }

        public void SendPacket(IPacket packet)
        {
            EnsureConnected();

            _worker.EnqueuePacket(packet);
        }

        private void EnsureNotConnected()
        {
            if (_connection != null)
                throw new ArgumentException("Already connected!");
        }

        private void EnsureConnected()
        {
            if(_connection == null)
                throw new ArgumentException("Not connected!");
        }

        public void SetState(ConnectionState state)
        {
            _logger.Information("Changing connection state: {state}", state);
            _worker.SetState(state);
        }
    }
}
