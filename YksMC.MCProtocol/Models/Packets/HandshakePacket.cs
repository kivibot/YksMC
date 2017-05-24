using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MCProtocol.Models.Attributes;
using YksMC.MCProtocol.Models.Types;

namespace YksMC.MCProtocol.Models.Packets
{
    [PacketId(0x00)]
    public class HandshakePacket : AbstractPacket
    {
        public VarInt ProtocolVersion { get; set; }
        public string ServerAddress { get; set; }
        public ushort ServerPort { get; set; }
        public VarInt NextState { get; set; }
    }
}
