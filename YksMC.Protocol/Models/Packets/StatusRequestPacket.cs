using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets
{
    public class StatusRequestPacket : IPacket
    {
        public VarInt Id { get; set; }

        public StatusRequestPacket()
        {
            Id = new VarInt(0x00);
        }
    }
}
