using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Packets.Play.Serverbound
{
    [Packet(0x0C, ConnectionState.Play, BoundTo.Server)]
    public class PlayerPositionPacket
    {
        public double X { get; set; }
        public double FeetY { get; set; }
        public double Z { get; set; }
        public bool OnGround { get; set; }
    }
}
