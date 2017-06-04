using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x27, ConnectionState.Play, BoundTo.Client)]
    public class EntityLookPacket
    {
        public VarInt EntityId { get; set; }
        public Angle Yaw { get; set; }
        public Angle Pitch { get; set; }
        public bool IsOnGround { get; set; }
    }
}
