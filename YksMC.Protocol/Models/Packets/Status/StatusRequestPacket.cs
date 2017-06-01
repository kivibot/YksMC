﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets.Status
{
    [Packet(0x00, ConnectionState.Status, BoundTo.Server)]
    public class StatusRequestPacket : IPacket
    {
        public VarInt Id { get; set; }

        public StatusRequestPacket()
        {
            Id = new VarInt(0x00);
        }
    }
}
