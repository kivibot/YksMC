using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets
{
    [Packet(0x00, ConnectionState.Handshake, BoundTo.Server)]
    public class HandshakePacket : IPacket
    {
        public VarInt Id { get; set; }
        public ProtocolVersion ProtocolVersion { get; set; }
        public string ServerAddress { get; set; }
        public ushort ServerPort { get; set; }
        public ConnectionState NextState { get; set; }

        public HandshakePacket()
        {
            Id = new VarInt(0x00);
        }
    }
}
