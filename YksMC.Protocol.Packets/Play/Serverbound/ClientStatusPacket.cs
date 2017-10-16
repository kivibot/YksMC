using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Serverbound
{
    [Packet(0x03, ConnectionState.Play, BoundTo.Server)]
    public class ClientStatusPacket
    {
        public VarInt ActionId { get; set; }

        public static int Respawn = 0;
        public static int RequestStatus = 1;
    }
}
