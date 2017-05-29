﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models.Packets
{
    public class StatusResponsePacket : IPacket
    {
        public VarInt Id { get; set; }
        public String JsonData { get; set; }
        public StatusResponsePacket()
        {
            Id = new VarInt(0x00);
        }
    }
}
