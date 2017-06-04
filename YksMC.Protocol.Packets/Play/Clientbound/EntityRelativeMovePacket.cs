using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x25, ConnectionState.Play, BoundTo.Client)]
    public class EntityRelativeMovePacket
    {
        public VarInt EntityId { get; set; }
        public Vector<short> DeltaPosition { get; set; }
        public bool IsOnGround { get; set; }
    }
}
