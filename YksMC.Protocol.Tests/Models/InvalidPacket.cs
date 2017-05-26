using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Protocol.Tests.Models
{
    public class InvalidPacket : AbstractPacket
    {
        public InvalidPacket UnsupportedType { get; set; }
    }
}
