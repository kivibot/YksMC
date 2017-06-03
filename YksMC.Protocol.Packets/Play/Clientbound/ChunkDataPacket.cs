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
    public class ChunkDataPacket : IPacket
    {
        public VarInt Id { get; set; }
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }
        public bool GroundUpContinuous { get; set; }
        public VarInt PrimaryBitMask { get; set; }
        public VarInt Size { get; set; }
        //TODO: data

        public ChunkDataPacket()
        {
            Id = new VarInt(0x20);
        }
    }
}
