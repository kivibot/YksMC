using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets.Login
{
    public class LoginSuccessPacket : IPacket
    {
        public VarInt Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }

        public LoginSuccessPacket()
        {
            Id = new VarInt(0x02);
        }
    }
}
