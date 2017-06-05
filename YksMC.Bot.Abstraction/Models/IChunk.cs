using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Models
{
    public interface IChunk
    {
        int X { get; }
        int Z { get; }
        Dimension Dimension { get; }

        Biome GetBiome(int x, int y);
        IBlock GetBlock(int x, int y, int z);
    }
}
