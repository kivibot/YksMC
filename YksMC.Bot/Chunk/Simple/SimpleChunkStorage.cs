using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;

namespace YksMC.Bot.Chunk.Simple
{
    public class SimpleChunkStorage : IChunkStorage
    {
        public IChunk CreateChunk(Dimension dimension, int x, int z)
        {
            throw new NotImplementedException();
        }

        public IBlock GetBlock(BlockLocation location)
        {
            throw new NotImplementedException();
        }

        public IChunk GetChunk(Dimension dimension, int x, int z)
        {
            throw new NotImplementedException();
        }

        public void SetBlockLight(BlockLocation location, byte level)
        {
            throw new NotImplementedException();
        }

        public void SetBlockType(BlockLocation location, IBlockType type)
        {
            throw new NotImplementedException();
        }

        public void SetSkyLight(BlockLocation location, byte level)
        {
            throw new NotImplementedException();
        }
    }
}
