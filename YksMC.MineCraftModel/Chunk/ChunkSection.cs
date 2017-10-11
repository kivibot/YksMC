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

        private readonly IBlock[,,] _blocks;

        public ChunkSection(IBlock emptyBlock)
        {
            IBlock[,,] blocks = new IBlock[Width, Height, Width];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int z = 0; z < Width; z++)
                    {
                        blocks[x, y, z] = emptyBlock;
                    }
                }
            }
            _blocks = blocks;
        }

        public ChunkSection(IBlock[,,] blocks)
        {
            _blocks = blocks;
        }

        public IBlock GetBlock(IBlockCoordinate position)
        {
            return _blocks[position.X, position.Y % Height, position.Z];
        }

        public ChunkSection ChangeBlock(IBlockCoordinate position, IBlock block)
        {
            IBlock[,,] blocks = (IBlock[,,])_blocks.Clone();
            blocks[position.X % Chunk.Width, position.Y % Height, position.Z % Chunk.Width] = block;
            return new ChunkSection(blocks);
        }
    }
}
