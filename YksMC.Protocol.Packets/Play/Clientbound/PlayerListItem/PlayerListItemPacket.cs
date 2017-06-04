using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound.PlayerListItem
{
    [Packet(0x2d, ConnectionState.Play, BoundTo.Client)]
    public class PlayerListItemPacket
    {
        public VarInt Action { get; set; }
        
    }
}
