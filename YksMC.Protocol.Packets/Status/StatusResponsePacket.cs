﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Status
{
    [Packet(0x00, ConnectionState.Status, BoundTo.Client)]
    public class StatusResponsePacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public String JsonData { get; set; }

        public StatusResponsePacket()
        {
            PacketId = new VarInt(0x00);
        }
    }
}
