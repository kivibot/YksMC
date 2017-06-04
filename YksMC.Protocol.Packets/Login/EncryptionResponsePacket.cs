﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Login
{
    [Packet(0x01, ConnectionState.Login, BoundTo.Server)]
    public class EncryptionResponsePacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public VarArray<byte> SharedSecret { get; set; }
        public VarArray<byte> VerifyToken { get; set; }

        public EncryptionResponsePacket()
        {
            PacketId = new VarInt(0x01);
        }
    }
}
