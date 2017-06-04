﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x39, ConnectionState.Play, BoundTo.Client)]
    public class EntityMetadataPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public VarInt EntityId { get; set; }
        //TODO: metadata

        public EntityMetadataPacket()
        {
            PacketId = new VarInt(0x39);
        }
    }
}
