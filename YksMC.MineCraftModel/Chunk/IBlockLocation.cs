using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Common;

namespace YksMC.MinecraftModel.Chunk
{
    public interface IBlockLocation
    {
        int X { get; }
        int Y { get; }
        int Z { get; }

        IBlockLocation Add(int x, int y, int z);
        IVector3<double> AsVector();
    }
}
