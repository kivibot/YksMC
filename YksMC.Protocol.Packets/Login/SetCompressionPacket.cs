﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Login
{
    [Packet(0x03, ConnectionState.Login, BoundTo.Client)]
    public class SetCompressionPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public VarInt Threshold { get; set; }

        public SetCompressionPacket()
        {
            PacketId = new VarInt(0x03);
        }
    }
}