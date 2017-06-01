using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets.Login
{
    public class EncryptionRequestPacket : IPacket
    {
        public VarInt Id { get; set; }
        public ByteArray PublicKey { get; set; }
        public ByteArray VerifyToken { get; set; }

        public EncryptionRequestPacket()
        {
            Id = new VarInt(0x01);
        }

    }
}
