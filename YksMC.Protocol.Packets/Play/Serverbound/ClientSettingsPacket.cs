using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Serverbound
{
    [Packet(0x04, ConnectionState.Play, BoundTo.Server)]
    public class ClientSettingsPacket
    {
        public string Locale { get; set; }
        public byte ViewDistance { get; set; }
        public VarInt ChatMode { get; set; }
        public bool UseChatColors { get; set; }
        public byte DisplayedSkinParts { get; set; }
        public VarInt MainHand { get; set; }
    }
}
