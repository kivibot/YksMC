using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Clients
{
    public interface IMCPacketClient
    {
        Task SendAsync(IPacket packet, CancellationToken cancelToken = default(CancellationToken));
        Task<T> ReceiveAsync<T>(CancellationToken cancelToken = default(CancellationToken)) where T : IPacket;
    }
}
