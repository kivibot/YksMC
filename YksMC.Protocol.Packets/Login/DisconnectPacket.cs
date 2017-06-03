using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Login
{
    [Packet(0x00, ConnectionState.Login, BoundTo.Client)]
    public class DisconnectPacket : IPacket
    {
        public VarInt Id { get; set; }
        public Chat Reason { get; set; }
    }
}
