using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets.Login
{
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
