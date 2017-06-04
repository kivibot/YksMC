using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x00, ConnectionState.Play, BoundTo.Client)]
    public class SpawnObjectPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public VarInt EntityId { get; set; }
        public Guid EntityUuid { get; set; }
        public byte Type { get; set; }
        public Vector<double> Position { get; set; }
        public Angle Pitch { get; set; }
        public Angle Yaw { get; set; }
        public uint Data { get; set; }
        public Vector<short> Velocity { get; set; }

        public SpawnObjectPacket()
        {
            PacketId = new VarInt(0x00);
        }
    }
}
