using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Chunk
{
    public interface IBlock
    {
        IBlockType Type { get; }
        byte BlockLight { get; }
        byte SkyLight { get; }
        Biome Biome { get; }
    }
}
