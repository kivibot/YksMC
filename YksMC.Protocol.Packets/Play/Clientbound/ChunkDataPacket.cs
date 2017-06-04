using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    [Packet(0x20, ConnectionState.Play, BoundTo.Client)]
    public class ChunkDataPacket 
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }
        public bool GroundUpContinuous { get; set; }
        public VarInt PrimaryBitMask { get; set; }
        public VarArray<VarInt, byte> DataAndBiomes { get; set; }
        //TODO: nbt tags
    }
}
