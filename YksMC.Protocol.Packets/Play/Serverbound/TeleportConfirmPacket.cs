﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Serverbound
{
    [Packet(0x00, ConnectionState.Play, BoundTo.Server)]
    public class TeleportConfirmPacket
    {
        public VarInt TeleportId { get; set; }
    }
}
