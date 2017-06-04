using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x16, ConnectionState.Play, BoundTo.Client)]
    public class SetWindowSlotPacket
    {
        public byte WindowId { get; set; }
        public short SlotId { get; set; }
        public WindowSlot Slot { get; set; }
    }
}