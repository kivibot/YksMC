using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets.Login
{
    public class SetCompressionPacket : IPacket
    {
        public VarInt Id { get; set; }
        public VarInt Threshold { get; set; }

        public SetCompressionPacket()
        {
            Id = new VarInt(0x03);
        }
    }
}
