﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Packets
{
    public class PongPacket : AbstractPacket
    {
        public long Payload { get; set; }
    }
}