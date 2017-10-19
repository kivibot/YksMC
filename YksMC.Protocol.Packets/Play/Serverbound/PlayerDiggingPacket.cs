using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Serverbound
{
    [Packet(0x13, ConnectionState.Play, BoundTo.Server)]
    public class PlayerDiggingPacket
    {
        public PlayerDiggingStatus Status { get; set; }
        public Position Location { get; set; }
        public byte Face { get; set; }
    }

    public enum PlayerDiggingStatus
    {
        StartedDigging = 0,
        CancelledDigging = 1,
        FinishedDigging = 2,
    }
}
