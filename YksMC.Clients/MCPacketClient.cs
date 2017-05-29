using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Serializing;

namespace YksMC.Clients
{
    public class MCPacketClient : IMCPacketClient
    {
        private readonly IMCPacketReader _reader;
        private readonly IMCPacketWriter _writer;
        private readonly IMCPacketDeserializer _deserializer;
        private readonly IMCPacketSerializer _serializer;

        public MCPacketClient(IMCPacketReader reader, IMCPacketWriter writer, IMCPacketDeserializer deserializer, IMCPacketSerializer serializer)
        {
            _reader = reader;
            _writer = writer;
            _deserializer = deserializer;
            _serializer = serializer;
        }

        public async Task<T> ReceiveAsync<T>(CancellationToken cancelToken = default(CancellationToken)) where T : IPacket
        {
            if (!await _reader.NextAsync(cancelToken))
                throw new InvalidOperationException("Connection closed.");

            return _deserializer.Deserialize<T>(_reader);
        }

        public async Task SendAsync(IPacket packet, CancellationToken cancelToken = default(CancellationToken))
        {
            _serializer.Serialize(packet, _writer);
            await _writer.SendPacketAsync(cancelToken);
        }
    }
}
