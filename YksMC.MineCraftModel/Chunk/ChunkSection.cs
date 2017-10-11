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
            return _blocks[position.X][position.Y % Height][position.Z];
        }

        public ChunkSection ChangeBlock(IBlockCoordinate position, IBlock block)
        {
            IBlock[][][] blocks = (IBlock[][][])_blocks.Clone();
            IBlock[][] slice = (IBlock[][])_blocks[position.X].Clone();
            IBlock[] column = (IBlock[])_blocks[position.X][position.Y].Clone();
            column[position.Z % Width] = block;
            slice[position.Y % Height] = column;
            blocks[position.X % Width] = slice;
            return new ChunkSection(blocks);
        }
    }
}
