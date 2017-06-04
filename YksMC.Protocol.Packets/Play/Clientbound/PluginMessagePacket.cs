﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x18, ConnectionState.Play, BoundTo.Client)]
    public class PluginMessagePacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public string Channel { get; set; }
        public VarArray<byte> Data { get; set; }

        public PluginMessagePacket()
        {
            PacketId = new VarInt(0x18);
        }
    }
}