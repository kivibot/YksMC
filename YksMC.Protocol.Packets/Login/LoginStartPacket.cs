using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Login
{
    [Packet(0x00, ConnectionState.Login, BoundTo.Server)]
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
