using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x49, ConnectionState.Play, BoundTo.Client)]
    public class EntityTeleportPacket 
    {
        public VarInt EntityId { get; set; }
        public Vector<double> Position { get; set; }
        public Angle Yaw { get; set; }
        public Angle Pitch { get; set; }
        public bool IsOnGround { get; set; }
    }
}
