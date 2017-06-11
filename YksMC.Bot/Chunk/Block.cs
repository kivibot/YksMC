using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Chunk;

namespace YksMC.Bot.Chunk
{
    public class Block : IBlock
    {
        public IBlockType Type { get; set; }
        public byte BlockLight { get; set; }
        public byte SkyLight { get; set; }
        public Biome Biome { get; set; }
    }
}
