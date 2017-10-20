using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Packets.Play.Serverbound
{
    [Packet(0x17, ConnectionState.Play, BoundTo.Server)]
    public class HeldItemChangePacket
    {
        public short Slot { get; set; }
    }
}
