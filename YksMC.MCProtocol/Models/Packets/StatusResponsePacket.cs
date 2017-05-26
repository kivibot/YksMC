using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MCProtocol.Models.Packets
{
    public class StatusResponsePacket : AbstractPacket
    {
        public String JsonData { get; set; }
    }
}
