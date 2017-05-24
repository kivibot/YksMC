using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MCProtocol.Models.Attributes;

namespace YksMC.MCProtocol.Models.Packets
{
    [PacketId(0x00)]
    public class LoginStartPacket : AbstractPacket
    {
        public string Name { get; set; }
    }
}
