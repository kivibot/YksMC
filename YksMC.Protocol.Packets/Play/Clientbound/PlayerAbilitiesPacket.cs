using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x2b, ConnectionState.Play, BoundTo.Client)]
    public class PlayerAbilitiesPacket
    {
        public byte Flags { get; set; }
        public float FlyingSpeed { get; set; }
        public float FieldOfViewModifier { get; set; }
    }
}
