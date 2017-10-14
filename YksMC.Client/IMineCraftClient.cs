using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Client.Models;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Packets;

namespace YksMC.Client
{
    public interface IMinecraftClient
    {
        ProtocolVersion ProtocolVersion { get; }
        ServerAddress Address { get; }
        ConnectionState State { get; }

        Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken));
        void Disconnect();
        void SendPacket(object packet);
        void SetState(ConnectionState state);

        event Action<object> PacketReceived;
    }
}
