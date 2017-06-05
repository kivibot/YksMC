using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.Models
{
    public class Chunk
    {
        public const int MaxSections = 32;

        public Dimension Dimension { get; set; }
        public int X { get; set; }
        public int Z { get; set; }
        public ChunkSection[] Sections { get; set; }
        public Biome[,] Biomes { get; set; }

        public Chunk(Dimension dimension, int x, int z)
        {
            Dimension = dimension;
            X = x;
            Z = z;
        }
    }
}
