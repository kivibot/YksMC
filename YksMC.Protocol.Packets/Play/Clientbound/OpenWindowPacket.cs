using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x13, ConnectionState.Play, BoundTo.Client)]
    public class OpenWindowPacket
    {
        public byte WindowId { get; set; }
        public string WindowType { get; set; }
        public Chat WindowTitle { get; set; }
        public byte WindowSlotCount { get; set; }
    }
}
