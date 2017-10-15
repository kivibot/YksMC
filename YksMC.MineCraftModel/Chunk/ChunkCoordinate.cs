using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.MinecraftModel.Chunk
{
    [DebuggerDisplay("[{X}, {Z}]")]
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

        public ChunkCoordinate(IBlockCoordinate blockPosition)
        {
            _x = (int)Math.Floor(blockPosition.X / (double)Chunk.Width);
            _z = (int)Math.Floor(blockPosition.Z / (double)Chunk.Width);
        }

        public override bool Equals(object obj)
        {
            IChunkCoordinate other = obj as IChunkCoordinate;
            if(other == null)
            {
                return false;
            }
            return other.X == _x && other.Z == _z;
        }

        public override int GetHashCode()
        {
            return 71 * (71 * (7 + _x) + _z);
        }
    }
}
