using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets.Status
{
    public class PingPacket : IPacket
    {
        public VarInt Id { get; set; }
        public long Payload { get; set; }

        public PingPacket()
        {
            Id = new VarInt(0x01);
        }
    }
}
