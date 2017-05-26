using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MCProtocol.Models.Packets;
using YksMC.MCProtocol.Models.Types;

namespace YksMc.Protocol.Tests.Models
{
    public class TestPacket : AbstractPacket
    {
        public bool Bool { get; set; }
        public sbyte SignedByte { get; set; }
        public byte Byte { get; set; }
        public short Short { get; set; }
        public ushort UnsignedShort { get; set; }
        public int Int { get; set; }
        public uint UnsignedInt { get; set; }
        public long Long { get; set; }
        public ulong UnsignedLong { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public string String { get; set; }
        public Chat Chat { get; set; }
        public VarInt VarInt { get; set; }
        public VarLong VarLong { get; set; }
        public Position Position { get; set; }
        public Angle Angle { get; set; }
        public Guid Guid { get; set; }
    }
}
