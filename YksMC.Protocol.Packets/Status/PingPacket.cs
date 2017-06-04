using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Status
{
    [Packet(0x01, ConnectionState.Status, BoundTo.Server)]
    public class PingPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public long Payload { get; set; }

        public PingPacket()
        {
            PacketId = new VarInt(0x01);
        }
    }
}
