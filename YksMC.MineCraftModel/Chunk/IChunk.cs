using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.MinecraftModel.Chunk
{
    public interface IChunk
    {
        T GetBlock<T>(IBlockLocation position) where T : class, IBlock;

        IChunk ChangeBlock(IBlockLocation coordinate, IBlock block);
    }
}
