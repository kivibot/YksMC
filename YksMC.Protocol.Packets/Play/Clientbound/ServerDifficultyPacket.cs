using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x0d, ConnectionState.Play, BoundTo.Client)]
    public class ServerDifficultyPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public byte Difficulty { get; set; }

        public ServerDifficultyPacket()
        {
            PacketId = new VarInt(0x0d);
        }
    }
}