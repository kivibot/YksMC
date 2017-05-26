using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MCProtocol.Models.Packets;

namespace YksMc.Protocol.Tests.Models
{
    public class InvalidPacket : AbstractPacket
    {
        public InvalidPacket UnsupportedType { get; set; }
    }
}
