using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YksMC.MinecraftModel.Block
{
    [DebuggerDisplay("[{X}, {Y}, {Z}]")]
    public class BlockCoordinate : IBlockCoordinate
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public int X => _x;
        public int Y => _y;
        public int Z => _z;

        public BlockCoordinate(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = Z;
        }

        public IBlockCoordinate Add(int x, int y, int z)
        {
            return new BlockCoordinate(_x + x, _y + y, _z + z);
        }
    }
}
