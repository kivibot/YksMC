using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Clients
{
    public delegate Owned<IMCPacketClient> IMCPacketClientFactory();

    public interface IMCPacketClient
    {

        Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken));
        Task SendAsync(IPacket packet, CancellationToken cancelToken = default(CancellationToken));
        Task<T> ReceiveAsync<T>(CancellationToken cancelToken = default(CancellationToken)) where T : IPacket;
    }
}
