using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YksMC.MCProtocol
{
    public class StreamMCPacketSource : IMCPacketSource
    {
        public Task<byte[]> GetNextAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
