using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x44, ConnectionState.Play, BoundTo.Client)]
    public class TimeUpdatePacket
    {
        public ulong WorldAge { get; set; }
        public ulong TimeOfDay { get; set; }
    }
}
