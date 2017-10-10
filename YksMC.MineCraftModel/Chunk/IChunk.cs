using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.MinecraftModel.Chunk
{
    public interface IChunk
    {
        IBlock GetBlock(IBlockCoordinate position);

        IChunk ChangeBlock(IBlockCoordinate coordinate, IBlock block);
    }
}
