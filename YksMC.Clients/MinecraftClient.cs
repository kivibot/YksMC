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

        public ProtocolVersion ProtocolVersion => _protocolVersion;

        public Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SendPacketAsync(IPacket packet, CancellationToken cancelToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
