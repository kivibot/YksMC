using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.World;

namespace YksMC.Abstraction.Chunk
{
    public interface IChunk
    {
        Dimension Dimension { get; }
        int X { get; }
        int Z { get; }
    }
}
