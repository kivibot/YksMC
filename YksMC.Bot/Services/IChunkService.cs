using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Models;

namespace YksMC.Bot.Services
{
    public interface IChunkService
    {
        Chunk CreateChunk(Dimension dimension, int chunkX, int chunkZ);
        Chunk GetChunk(Dimension dimension, int chunkX, int chunkZ);

        void SetBiome(Chunk chunk, int x, int z, Biome biome);
        void SetBlockType(Chunk chunk, int x, int y, int z, BlockType type);
        void SetBlockLight(Chunk chunk, int x, int y, int z, byte light);
        void SetSkyLight(Chunk chunk, int x, int y, int z, byte light);

    }
}
