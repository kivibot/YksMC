using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets
{
    public class HandshakePacket : IPacket
    {
        public const int PacketId = 0x00;

        public VarInt Id { get; set; }
        public ProtocolVersion ProtocolVersion { get; set; }
        public string ServerAddress { get; set; }
        public ushort ServerPort { get; set; }
        public ConnectionState NextState { get; set; }

        public HandshakePacket()
        {
            Id = new VarInt(PacketId);
        }
    }
}
