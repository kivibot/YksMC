using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x10, ConnectionState.Play, BoundTo.Client)]
    public class MultiBlockChangePacket
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }
        public VarArray<VarInt, MultiBlockChangePacketRecord> Records { get; set; }
    }

    public class MultiBlockChangePacketRecord
    {
        public byte HorizontalPosition { get; set; }
        public byte Y { get; set; }
        public VarInt BlockId { get; set; }
    }
}
