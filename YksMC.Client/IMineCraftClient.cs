using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Client.Models.Status;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Client
{
    public interface IMinecraftClient
    {
        ProtocolVersion ProtocolVersion { get; }

        Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken));
        void SendPacket(IPacket packet);
        void SetState(ConnectionState state);
    }
}
