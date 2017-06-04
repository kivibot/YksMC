using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x0f, ConnectionState.Play, BoundTo.Client)]
    public class ChatMessagePacket
    {
        public Chat Chat { get; set; }
        public byte Position { get; set; }
    }
}
