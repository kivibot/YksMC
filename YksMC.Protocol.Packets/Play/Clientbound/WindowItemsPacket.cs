using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x14, ConnectionState.Play, BoundTo.Client)]
    public class WindowItemsPacket 
    {
        public byte WindowId { get; set; }
        public VarArray<WindowSlot> Slots { get; set; }
    }
}
