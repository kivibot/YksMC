using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Chunk
{
    public class ChunkCoordinate : IChunkCoordinate
    {
        private readonly int _x;
        private readonly int _z;

        public int X => _x;
        public int Z => _z;

        public ChunkCoordinate(int x, int z)
        {
            _x = x;
            _z = z;
        }
    }
}
