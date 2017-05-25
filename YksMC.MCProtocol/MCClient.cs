using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YksMC.MCProtocol
{
    public class MCClient : IMCClient
    {
        private readonly IMCPacketDeserializer _deserializer;
        private readonly IMCPacketReader _packetReader;

        public MCClient(IMCPacketDeserializer deserializer, IMCPacketReader packetReader)
        {
            _deserializer = deserializer;
            _packetReader = packetReader;
        }
    }
}
