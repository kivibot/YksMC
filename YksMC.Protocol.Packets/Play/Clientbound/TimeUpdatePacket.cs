using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x44, ConnectionState.Play, BoundTo.Client)]
    public class TimeUpdatePacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public long WorldAge { get; set; }
        public long TimeOfDay { get; set; }

        public TimeUpdatePacket()
        {
            PacketId = new VarInt(0x44);
        }
    }
}
