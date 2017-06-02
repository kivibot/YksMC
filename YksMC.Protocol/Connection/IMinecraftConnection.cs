using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YksMC.Protocol.Connection
{
    public interface IMinecraftConnection
    {
        Task SendPacketAsync(byte[] data, CancellationToken cancelToken = default(CancellationToken));
        Task<byte[]> ReceivePacketAsync(CancellationToken cancelToken = default(CancellationToken));
        void EnableCompression(int threshold);
    }
}
