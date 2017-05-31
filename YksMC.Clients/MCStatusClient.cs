using Autofac.Features.OwnedInstances;
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
    public class MCStatusClient : IMCStatusClient
    {
        private readonly IMCPacketClientFactory _clientFactory;

        public MCStatusClient(IMCPacketClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ServerStatus> GetStatusAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken))
        {
            using (Owned<IMCPacketClient> client  = _clientFactory())
            {
                return await GetStatusInternalAsync(host, port, client.Value, cancelToken);
            }
        }

        private async Task<ServerStatus> GetStatusInternalAsync(string host, ushort port, IMCPacketClient client, CancellationToken cancelToken)
        {
            await client.ConnectAsync(host, port, cancelToken);

            await SendHandshakeAsync(host, port, client, cancelToken);
            await SendStatusRequestAsync(client, cancelToken);

            StatusResponsePacket responsePacket = await client.ReceiveAsync<StatusResponsePacket>(cancelToken);

            ServerStatus status = JsonConvert.DeserializeObject<ServerStatus>(responsePacket.JsonData);

            await SendPingAsync(client, cancelToken);

            PongPacket pongPacket = await client.ReceiveAsync<PongPacket>(cancelToken);

            status.Ping = TimeSpan.FromTicks(DateTimeOffset.UtcNow.Ticks - pongPacket.Payload);

            return status;
        }

        private static async Task SendPingAsync(IMCPacketClient client, CancellationToken cancelToken)
        {
            await client.SendAsync(new PingPacket()
            {
                Payload = DateTimeOffset.UtcNow.Ticks
            }, cancelToken);
        }

        private static async Task SendStatusRequestAsync(IMCPacketClient client, CancellationToken cancelToken)
        {
            await client.SendAsync(new StatusRequestPacket(), cancelToken);
        }

        private static async Task SendHandshakeAsync(string host, ushort port, IMCPacketClient client, CancellationToken cancelToken)
        {
            await client.SendAsync(new HandshakePacket()
            {
                ProtocolVersion = ProtocolVersion.Unknown,
                ServerAddress = host,
                ServerPort = port,
                NextState = SubProtocol.Status
            }, cancelToken);
        }
    }
}
