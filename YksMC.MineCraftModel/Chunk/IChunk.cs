using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.MinecraftModel.Chunk
{
    public interface IChunk
    {
        IBlock GetBlock(BlockCoordinate position);

        IChunk ChangeBlock(BlockCoordinate coordinate, IBlock block);
    }
}
