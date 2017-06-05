using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;

namespace YksMC.Abstraction.Services
{
    public interface IChunkService
    {
        IChunk GetChunk(Dimension dimension, int x, int z);

        void SetBlockType(IBlock block, IBlockType type);
        void SetBlockLight(IBlock block);
        void SetSkyLight(IBlock block);
    }
}
