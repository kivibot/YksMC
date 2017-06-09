using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.World;

namespace YksMC.Abstraction.Chunk
{
    public interface IChunk
    {
        int X { get; }
        int Z { get; }
        Dimension Dimension { get; }
        bool IsLoaded { get; }
    }
}
