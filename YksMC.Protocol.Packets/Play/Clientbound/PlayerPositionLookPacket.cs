using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x2e, ConnectionState.Play, BoundTo.Client)]
    public class PlayerPositionLookPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public double X { get; set; }
        public double FeetY { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool IsOnGround { get; set; }

        public PlayerPositionLookPacket()
        {
            PacketId = new VarInt(0x2e);
        }
    }
}
