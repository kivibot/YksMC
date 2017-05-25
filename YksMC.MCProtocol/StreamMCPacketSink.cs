using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.MCProtocol.Utils;

namespace YksMC.MCProtocol
{
    public class StreamMCPacketSink : IMCPacketSink, IDisposable
    {
        private readonly Stream _stream;

        public StreamMCPacketSink(Stream stream)
        {
            _stream = stream;
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public async Task SendPacketAsync(byte[] data, CancellationToken cancelToken = default(CancellationToken))
        {
            byte[] packetLenghtData = VarIntUtil.EncodeVarInt(data.Length);
            await _stream.WriteAsync(packetLenghtData, 0, packetLenghtData.Length, cancelToken);
            await _stream.WriteAsync(data, 0, data.Length);
        }
    }
}
