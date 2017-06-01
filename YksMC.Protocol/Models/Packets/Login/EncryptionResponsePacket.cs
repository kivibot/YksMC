using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets.Login
{
    [Packet(0x01, ConnectionState.Login, BoundTo.Server)]
    public class EncryptionResponsePacket : IPacket
    {
        public VarInt Id { get; set; }
        public ByteArray SharedSecret { get; set; }
        public ByteArray VerifyToken { get; set; }

        public EncryptionResponsePacket()
        {
            Id = new VarInt(0x01);
        }
    }
}
