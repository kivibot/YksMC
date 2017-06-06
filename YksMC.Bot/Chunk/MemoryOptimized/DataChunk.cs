using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;

namespace YksMC.Bot.Chunk.MemoryOptimized
{
    public class DataChunk : IChunk
    {
        public DataBlock[,,] Blocks { get; set; }

        public Dimension Dimension { get; }
        public int X { get; }
        public int Z { get; }

        public DataChunk(Dimension dimension,int x, int z)
        {
            Dimension = dimension;
            X = x;
            Z = z;
        }
    }
}
