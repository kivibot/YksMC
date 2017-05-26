using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets
{
    public class HandshakePacket : AbstractPacket
    {
        public const int PacketId = 0x00;

        public VarInt ProtocolVersion { get; set; }
        public string ServerAddress { get; set; }
        public ushort ServerPort { get; set; }
        public VarInt NextState { get; set; }
    }
}
