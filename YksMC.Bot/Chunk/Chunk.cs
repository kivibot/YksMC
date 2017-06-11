using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Chunk;
using YksMC.Abstraction.World;

namespace YksMC.Bot.Chunk
{
    public class Chunk : IChunk
    {
        public Dimension Dimension { get; set; }
        public int X { get; set; }
        public int Z { get; set; }

        public Biome[] Biomes { get; set; }
        public IBlockType[] BlockTypes { get; set; }
        public byte[] BlockLightLevels { get; set; }
        public byte[] SkyLightLevels { get; set; }
    }
}
