using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Packets.Play.Common
{
    [Packet(0x05, ConnectionState.Play, BoundTo.Server)]
    [Packet(0x11, ConnectionState.Play, BoundTo.Client)]
    public class ConfirmTransactionPacket
    {
        public byte WindowId { get; set; }
        public short ActionNumber { get; set; }
        public bool Accepted { get; set; }
    }
}
