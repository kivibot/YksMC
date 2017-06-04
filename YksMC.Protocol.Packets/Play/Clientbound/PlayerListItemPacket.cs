using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x2d, ConnectionState.Play, BoundTo.Client)]
    public class PlayerListItemPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public VarInt Action { get; set; }
        public VarInt NumberOfPlayers { get; set; }
        //TODO: Player field

        public PlayerListItemPacket()
        {
            PacketId = new VarInt(0x2d);
        }
    }
}
