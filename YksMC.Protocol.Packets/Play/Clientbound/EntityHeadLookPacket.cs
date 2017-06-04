using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x34, ConnectionState.Play, BoundTo.Client)]
    public class EntityHeadLookPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public VarInt EntityId { get; set; }
        public Angle HeadYaw { get; set; }

        public EntityHeadLookPacket()
        {
            PacketId = new VarInt(0x34);
        }
    }
}
