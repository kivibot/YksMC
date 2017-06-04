using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.Models
{
    public class Chunk
    {
        public Dimension Dimension { get; set; }
        public int X { get; set; }
        public int Z { get; set; }
        public List<ChunkSection> Sections { get; set; }
        public Biome[,] Biomes { get; set; }
    }
}
