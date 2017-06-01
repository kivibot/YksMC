using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets.Login
{
    public class LoginStartPacket : IPacket
    {
        public VarInt Id { get; set; }
        public string Name { get; set; }

        public LoginStartPacket()
        {
            Id = new VarInt(0x00);
        }
    }
}
