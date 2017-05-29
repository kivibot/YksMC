using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Tests.Fakes
{
    public class FakeMCConnection : IMCConnection
    {
        private readonly List<byte[]> _sentPackets = new List<byte[]>();
        private readonly Queue<byte[]> _queue;

        public IReadOnlyList<byte[]> SentPackets => _sentPackets;
        public Queue<byte[]> Queue => _queue;

        public FakeMCConnection()
        {
            _queue = new Queue<byte[]>();
        }

        public FakeMCConnection(params byte[][] array)
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

        public Task SendPacketAsync(byte[] data, CancellationToken cancelToken = default(CancellationToken))
        {
            _sentPackets.Add(data);
            return Task.FromResult(true);
        }
    }
}
