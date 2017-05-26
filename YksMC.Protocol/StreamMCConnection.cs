using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Exceptions;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Utils;

namespace YksMC.Protocol
{
    public class StreamMCConnection : IMCPacketSource, IMCPacketSink, IDisposable
    {
        private readonly Stream _stream;

        public StreamMCConnection(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            _stream = stream;
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public async Task<byte[]> GetNextAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            VarInt length = await ReadVarIntAsync(cancelToken);
            if (length == null)
                return null;
            return await ReadBytesAsync(length.Value, cancelToken);
        }

        private async Task<VarInt> ReadVarIntAsync(CancellationToken cancelToken)
        {
            byte[] data = new byte[VarInt.MaxLength];
            for (int i = 0; i < VarInt.MaxLength; i++)
            {
                int bytesRead = await _stream.ReadAsync(data, i, 1);
                if (bytesRead == 0)
                {
                    if (i == 0)
                        return null;
                    else
                        throw new PacketSourceException("Unexpected EOT");
                }
                if ((data[i] & 0b10000000) == 0)
                    return new VarInt(VarIntUtil.DecodeVarInt(data));
            }
            throw new PacketSourceException("Invalid VarInt (too long)");
        }

        private async Task<byte[]> ReadBytesAsync(int length, CancellationToken cancelToken)
        {
            byte[] data = new byte[length];
            for (int totalBytesRead = 0; totalBytesRead < length;)
            {
                int bytesRead = await _stream.ReadAsync(data, totalBytesRead, length - totalBytesRead);
                if (bytesRead == 0)
                    throw new PacketSourceException("Unexpected EOT");
                totalBytesRead += bytesRead;
            }
            return data;
        }

        public async Task SendPacketAsync(byte[] data, CancellationToken cancelToken = default(CancellationToken))
        {
            byte[] packetLenghtData = VarIntUtil.EncodeVarInt(data.Length);
            await _stream.WriteAsync(packetLenghtData, 0, packetLenghtData.Length, cancelToken);
            await _stream.WriteAsync(data, 0, data.Length);
        }
    }
}
