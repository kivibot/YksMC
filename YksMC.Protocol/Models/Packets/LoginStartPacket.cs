using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets
{
    [PacketId(0x00)]
    public class LoginStartPacket : IPacket
    {
        public VarInt Id { get; set; }
        public string Name { get; set; }
    }
}
