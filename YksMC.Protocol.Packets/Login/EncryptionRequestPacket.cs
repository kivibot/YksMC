using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Login
{
    [Packet(0x01, ConnectionState.Login, BoundTo.Client)]
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
