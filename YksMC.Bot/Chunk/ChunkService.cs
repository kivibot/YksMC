using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;
using YksMC.Abstraction.Services;

namespace YksMC.Bot.Chunk
{
    public class ChunkService : IChunkService
    {
        private IChunkStorage _storage;

        public ChunkService(IChunkStorage storage)
        {
            _storage = storage;
        }

        public IChunk CreateChunk(Dimension dimension, int x, int z)
        {
            return _storage.CreateChunk(dimension, x, z);
        }

        public IBlock GetBlock(BlockLocation location)
        {
            return _storage.GetBlock(location);
        }

        public IChunk GetChunk(Dimension dimension, int x, int z)
        {
            return _storage.GetChunk(dimension, x, z);
        }

        public void SetBlockLight(BlockLocation location, byte level)
        {
            _storage.SetBlockLight(location, level);
        }

        public void SetBlockType(BlockLocation location, IBlockType type)
        {
            _storage.SetBlockType(location, type);
        }

        public void SetSkyLight(BlockLocation location, byte level)
        {
            _storage.SetSkyLight(location, level);
        }
    }
}
