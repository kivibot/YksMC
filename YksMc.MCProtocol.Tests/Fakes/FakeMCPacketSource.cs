using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.MCProtocol;

namespace YksMc.Protocol.Tests.Fakes
{
    public class FakeMCPacketSource : IMCPacketSource
    {
        private readonly Queue<byte[]> _queue;

        public FakeMCPacketSource(params byte[][] array)
        {
            _queue = new Queue<byte[]>(array);
        }

        public Task<byte[]> GetNextAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            if (_queue.Count == 0)
                return Task.FromResult<byte[]>(null);
            byte[] data = _queue.Dequeue();
            return Task.FromResult(data);
        }
    }
}
