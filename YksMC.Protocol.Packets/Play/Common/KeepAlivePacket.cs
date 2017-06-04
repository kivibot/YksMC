using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Common
{
    [Packet(0x1F, ConnectionState.Play, BoundTo.Client)]
    [Packet(0x0B, ConnectionState.Play, BoundTo.Server)]
    public class KeepAlivePacket
    {
        public VarInt KeepAliveId { get; set; }
    }
}
