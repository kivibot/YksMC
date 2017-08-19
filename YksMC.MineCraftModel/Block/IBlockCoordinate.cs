using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Block
{
    public interface IBlockCoordinate
    {
        int X { get; }
        int Y { get; }
        int Z { get; }

        IBlockCoordinate Add(int x, int y, int z);
    }
}
