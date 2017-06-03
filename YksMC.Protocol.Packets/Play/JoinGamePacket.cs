using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play
{
    [Packet(0x23, ConnectionState.Play, BoundTo.Client)]
    public class JoinGamePacket : IPacket
    {
        public VarInt Id { get; set; }
        public byte Gamemode { get; set; }
        public int Dimension { get; set; }
        public byte Difficulty { get; set; }
        public byte MaxPlayers { get; set; }
        public string LevelType { get; set; }
        public bool ReducedDebugInfo { get; set; }

        public JoinGamePacket()
        {
            Id = new VarInt(0x23);
        }
    }
}
