﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x4a, ConnectionState.Play, BoundTo.Client)]
    public class EntityProperties
    {
        public VarInt EntityId { get; set; }
        public VarArray<VarInt, EntityProperty> Properties { get; set; }
    }
}
