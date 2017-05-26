using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;

namespace YksMC.Protocol.Models.Packets
{
    [PacketId(0x00)]
    public class LoginStartPacket : AbstractPacket
    {
        public string Name { get; set; }
    }
}
