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
            IMCPacketClient client = _clientFactory.Create();
            try
            {
                return await GetStatusInternalAsync(host, port, client, cancelToken);
            }
            finally
            {
                _clientFactory.Release(client);
            }
        }

        private async Task<ServerStatus> GetStatusInternalAsync(string host, ushort port, IMCPacketClient client, CancellationToken cancelToken)
        {
            await client.SendAsync(new HandshakePacket()
            {
                ProtocolVersion = ProtocolVersion.Unknown,
                ServerAddress = host,
                ServerPort = port,
                NextState = SubProtocol.Status
            }, cancelToken);
            await client.SendAsync(new StatusRequestPacket(), cancelToken);

            StatusResponsePacket responsePacket = await client.ReceiveAsync<StatusResponsePacket>(cancelToken);

            await client.SendAsync(new PingPacket()
            {
                Payload = DateTimeOffset.UtcNow.Ticks
            }, cancelToken);

            PongPacket pongPacket = await client.ReceiveAsync<PongPacket>(cancelToken);
            TimeSpan ping = TimeSpan.FromTicks(DateTimeOffset.UtcNow.Ticks - pongPacket.Payload);

            ServerStatus status = JsonConvert.DeserializeObject<ServerStatus>(responsePacket.JsonData);
            status.Ping = ping;

            return status;
        }
    }
}
