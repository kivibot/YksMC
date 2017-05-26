using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YksMC.Protocol
{
    public interface IMCPacketSource
    {
        Task<byte[]> GetNextAsync(CancellationToken cancelToken = default(CancellationToken));
    }
}
