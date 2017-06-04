using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x43, ConnectionState.Play, BoundTo.Client)]
    public class SpawnPositionPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public Position Location { get; set; }

        public SpawnPositionPacket()
        {
            PacketId = new VarInt(0x43);
        }
    }
}