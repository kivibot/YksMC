using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.MinecraftModel.Chunk
{
    public class ChunkSection
    {
        public const int Height = 16;
        public const int Width = 16;

        private readonly IBlock[][][] _blocks;

        public ChunkSection(IBlock emptyBlock)
        {
            IBlock[] emptyRow = new IBlock[Width];
            for (int i = 0; i < Width; i++)
            {
                emptyRow[i] = emptyBlock;
            }

            IBlock[][] emptySlice = new IBlock[Height][];
            for(int i=0; i<Height; i++)
            {
                emptySlice[i] = emptyRow;
            }

            IBlock[][][] blocks = new IBlock[Width][][];
            for(int i=0; i<Width; i++)
            {
                blocks[i] = emptySlice;
            }

            _blocks = blocks;
        }

        public ChunkSection(IBlock[][][] blocks)
        {
            _blocks = blocks;
        }

        public IBlock GetBlock(IBlockCoordinate position)
        {
            return _blocks[Mod(position.X, ChunkSection.Width)][Mod(position.Y, ChunkSection.Height)][Mod(position.Z, ChunkSection.Width)];
        }

        public ChunkSection ChangeBlock(IBlockCoordinate position, IBlock block)
        {
            int localX = Mod(position.X, Chunk.Width);
            int localY = Mod(position.Y, Chunk.Height);
            int localZ = Mod(position.Z, Chunk.Width);
            IBlock[][][] blocks = (IBlock[][][])_blocks.Clone();
            IBlock[][] slice = (IBlock[][])_blocks[localZ].Clone();
            IBlock[] column = (IBlock[])_blocks[localZ][localY].Clone();
            column[localX] = block;
            slice[localY] = column;
            blocks[localZ] = slice;
            return new ChunkSection(blocks);
        }

        [Obsolete("Use a library implemention instead")]
        private int Mod(int x, int m)
        {
            var a = (x % m + m) % m;
            return a;
        }
    }
}
