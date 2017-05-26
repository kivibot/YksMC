using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Packets
{
    public class PingPacket : AbstractPacket
    {
        public long Payload { get; set; }
    }
}
