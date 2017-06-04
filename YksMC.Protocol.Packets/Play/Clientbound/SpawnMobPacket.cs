using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x03, ConnectionState.Play, BoundTo.Client)]
    public class SpawnMobPacket
    {
        public VarInt EntityId { get; set; }
        public Guid EntityGuid { get; set; }
        public VarInt Type { get; set; }
        public Vector<double> Position { get; set; }
        public Angle Yaw { get; set; }
        public Angle Pitch { get; set; }
        public Angle HeadPitch { get; set; }
        public short VelocityX { get; set; }
        public short VelocityY { get; set; }
        public short VelocityZ { get; set; }

        //TODO: metadata
    }
}
