using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.MinecraftModel.Chunk
{
    public class ChunkSection
    {
        public const int Height = 16;

        private readonly IBlock[,,] _blocks;

        public ChunkSection(IBlock[,,] blocks)
        {
            _blocks = blocks;
        }

        public IBlock GetBlock(BlockCoordinate position)
        {
            return _blocks[position.X, position.Y % Height, position.Z];
        }

        public ChunkSection ChangeBlock(BlockCoordinate position, IBlock block)
        {
            IBlock[,,] blocks = (IBlock[,,])_blocks.Clone();
            blocks[position.X % Chunk.Width, position.Y % Height, position.Z % Chunk.Width] = block;
            return new ChunkSection(blocks);
        }
    }
}
