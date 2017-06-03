using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x37, ConnectionState.Play, BoundTo.Client)]
    public class HeldItemChangePacket : IPacket
    {
        public VarInt Id { get; set; }
        public byte Slot { get; set; }

        public HeldItemChangePacket()
        {
            Id = new VarInt(0x37);
        }
    }
}
