using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.World;

namespace YksMC.Abstraction.Chunk
{
    public interface IChunkService
    {
        IChunk CreateChunk(Dimension dimension, int x, int z);
        IBlock GetBlock(BlockLocation location);

        void SetBiome(Dimension dimension, int x, int z, Biome biome);
        void SetBlockType(BlockLocation location, IBlockType type);
        void SetBlockLight(BlockLocation location, byte lightLevel);
        void SetSkyLight(BlockLocation location, byte lightLevel);
    }
}
