using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x3e, ConnectionState.Play, BoundTo.Client)]
    public class UpdateHealthPacket : IPacket
    {
        public VarInt PacketId { get; set; }
        public float Health { get; set; }
        public VarInt Food { get; set; }
        public float FoodSaturation { get; set; }

        public UpdateHealthPacket()
        {
            PacketId = new VarInt(0x3e);
        }
    }
}
