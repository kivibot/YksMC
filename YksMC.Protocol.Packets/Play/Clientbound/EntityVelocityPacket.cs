using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x3b, ConnectionState.Play, BoundTo.Client)]
    public class EntityVelocityPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public VarInt EntityId { get; set; }
        public Vector<short> Velocity { get; set; }
    }
}
