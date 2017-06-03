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
    public class SpawnMobPacket : IPacket
    {
        public VarInt Id { get; set; }
        public VarInt EntityId { get; set; }
        public Guid EntityGuid { get; set; }
        public VarInt Type { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public Angle Yaw { get; set; }
        public Angle Pitch { get; set; }
        public Angle HeadPitch { get; set; }
        public short VelocityX { get; set; }
        public short VelocityY { get; set; }
        public short VelocityZ { get; set; }

        //TODO: metadata

        public SpawnMobPacket()
        {
            Id = new VarInt(0x03);
        }
    }
}
