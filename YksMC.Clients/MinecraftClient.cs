using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Clients.Models.Status;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Packets.Login;
using YksMC.Protocol.Models.Packets.Status;

namespace YksMC.Clients
{
    public class MinecraftClient : IMinecraftClient
    {
        private const ProtocolVersion _protocolVersion = ProtocolVersion.v1_11_2;

        private readonly IMCPacketClient _packetClient;
        private ConnectionState _state;
        private string _host;
        private ushort _port;
        private string _username = "PleaseInjectMe";

        public ProtocolVersion ProtocolVersion => _protocolVersion;

        public MinecraftClient(IMCPacketClient packetClient)
        {
            _packetClient = packetClient;
            SetState(ConnectionState.None);
        }

        public async Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken))
        {
            RequireState(ConnectionState.None);

            await _packetClient.ConnectAsync(host, port, cancelToken);
            _host = host;
            _port = port;
            SetState(ConnectionState.Handshake);
        }

        public async Task<ServerStatus> GetStatusAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            RequireState(ConnectionState.Handshake);

            await _packetClient.SendAsync(new HandshakePacket()
            {
                ProtocolVersion = ProtocolVersion.Unknown,
                ServerAddress = _host,
                ServerPort = _port,
                NextState = ConnectionState.Status
            }, cancelToken);
            SetState(ConnectionState.Status);

            await _packetClient.SendAsync(new StatusRequestPacket(), cancelToken);
            StatusResponsePacket responsePacket = await _packetClient.ReceiveAsync<StatusResponsePacket>(cancelToken);
            ServerStatus status = JsonConvert.DeserializeObject<ServerStatus>(responsePacket.JsonData);

            await _packetClient.SendAsync(new PingPacket() { Payload = DateTimeOffset.UtcNow.Ticks }, cancelToken);
            PongPacket pongPacket = await _packetClient.ReceiveAsync<PongPacket>(cancelToken);
            status.Ping = TimeSpan.FromTicks(DateTimeOffset.UtcNow.Ticks - pongPacket.Payload);

            return status;
        }

        public async Task LoginAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            RequireState(ConnectionState.Handshake);

            await _packetClient.SendAsync(new HandshakePacket()
            {
                ProtocolVersion = _protocolVersion,
                ServerAddress = _host,
                ServerPort = _port,
                NextState = ConnectionState.Login
            }, cancelToken);
            SetState(ConnectionState.Login);

            await _packetClient.SendAsync(new LoginStartPacket()
            {
                Name = _username
            }, cancelToken);

            while (true)
            {
                //TODO: remove the "switch"
                IPacket packet = await _packetClient.ReceiveAsync(cancelToken);
                if (packet is DisconnectPacket)
                    throw new ArgumentException("Disconnected: " + (packet as DisconnectPacket).Reason.Value);
                else if (packet is LoginSuccessPacket)
                    break;
                else
                    throw new ArgumentException("Compression or encryption not supported!");
            }
        }

        private void RequireState(ConnectionState requiredState)
        {
            if (_state != requiredState)
                throw new InvalidOperationException("Invalid connection state!");
        }

        private void SetState(ConnectionState state)
        {
            _state = state;
            _packetClient.SetState(state);
        }
    }
}
