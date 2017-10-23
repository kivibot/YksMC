using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Protocol.Packets.Play.Serverbound
{
    [Packet(0x07, ConnectionState.Play, BoundTo.Server)]
    public class ClickEmptyWindowSlotPacket
    {
        public byte WindowId { get; set; }
        public short Slot { get; set; }
        public byte Button { get; set; }
        public short ActionNumber { get; set; }
        public WindowClickMode Mode { get; set; }

        public short NoBlockId { get; } = -123;
    }

    public enum WindowClickMode
    {
        Click = 0,
        ShiftClick = 1,
        Number = 2,
        MiddleClick = 3,
        Drop = 4,
        Drag = 5,
        DoubleClick = 6
    }
}
