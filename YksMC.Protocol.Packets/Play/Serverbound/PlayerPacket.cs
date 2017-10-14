using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Packets.Play.Serverbound
{
    [Packet(0x0F, ConnectionState.Play, BoundTo.Server)]
    public class PlayerPacket
    {
        public bool OnGround { get; set; }
    }
}
