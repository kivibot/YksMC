using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Clients.Injection;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Types;

namespace YksMC.Clients
{
    public interface IMCPacketClient
    {
        Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken));
        Task SendAsync(IPacket packet, CancellationToken cancelToken = default(CancellationToken));
        Task<T> ReceiveAsync<T>(CancellationToken cancelToken = default(CancellationToken)) where T : IPacket;
        Task<IPacket> ReceiveAsync(CancellationToken cancelToken = default(CancellationToken));
        void SetState(ConnectionState state);
    }
}
