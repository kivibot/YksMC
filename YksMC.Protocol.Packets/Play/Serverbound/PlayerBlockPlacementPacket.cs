using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Serverbound
{
    [Packet(0x1C, ConnectionState.Play, BoundTo.Server)]
    public class PlayerBlockPlacementPacket
    {
        public Position Location { get; set; }
        public VarInt Face { get; set; }
        public VarInt Hand { get; set; }
        public float CursorX { get; set; }
        public float CursorY { get; set; }
        public float CursorZ { get; set; }
    }
}
