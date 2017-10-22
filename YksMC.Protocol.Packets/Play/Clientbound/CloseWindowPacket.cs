using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x12, ConnectionState.Play, BoundTo.Client)]
    public class CloseWindowPacket
    {
        public byte WindowId { get; set; }
    }
}
