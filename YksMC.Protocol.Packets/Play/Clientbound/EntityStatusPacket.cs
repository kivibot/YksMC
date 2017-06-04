using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x1b, ConnectionState.Play, BoundTo.Client)]
    public class EntityStatusPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public int EntityId { get; set; }
        public byte EntityStatus { get; set; }

        public EntityStatusPacket()
        {
            PacketId = new VarInt(0x1b);
        }
    }
}
