using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Models;

namespace YksMC.Bot.Services
{
    public class ChunkService : IChunkService
    {
        private readonly IBlockTypeService _blockTypeService;
        private readonly ChunkSection _emptySection;

        private Dictionary<Tuple<Dimension, int, int>, Chunk> _chunks = new Dictionary<Tuple<Dimension, int, int>, Chunk>();

        public ChunkService(IBlockTypeService blockTypeService)
        {
            _blockTypeService = blockTypeService;
            _emptySection = CreateEmptySection();
        }

        public Chunk CreateChunk(Dimension dimension, int chunkX, int chunkZ)
        {
            Chunk chunk = new Chunk(dimension, chunkX, chunkZ);
            chunk.Sections = new ChunkSection[Chunk.MaxSections];
            for (int i = 0; i < Chunk.MaxSections; i++)
            {
                chunk.Sections[i] = _emptySection;
            }
            chunk.Biomes = new Biome[ChunkSection.Width, ChunkSection.Width];
            _chunks.Add(new Tuple<Dimension, int, int>(dimension, chunkX, chunkZ), chunk);
            return chunk;
        }

        public Chunk GetChunk(Dimension dimension, int chunkX, int chunkZ)
        {
            if (!_chunks.TryGetValue(new Tuple<Dimension, int, int>(dimension, chunkX, chunkZ), out Chunk chunk))
                return null;
            return chunk;
        }

        public void SetBiome(Chunk chunk, int x, int z, Biome biome)
        {
            chunk.Biomes[x, z] = biome;
        }

        public void SetBlockLight(Chunk chunk, int x, int y, int z, byte light)
        {
            GetBlock(chunk, x, y, z, true).BlockLight = light;
        }

        public void SetBlockType(Chunk chunk, int x, int y, int z, BlockType type)
        {
            GetBlock(chunk, x, y, z, true).Type = type;
        }

        private Block GetBlock(Chunk chunk, int x, int y, int z, bool createSection)
        {
            ChunkSection section = GetSection(chunk, y / ChunkSection.Height, createSection);
            return section.Blocks[x, y % ChunkSection.Height, z];
        }

        private ChunkSection GetSection(Chunk chunk, int y, bool create)
        {
            ChunkSection section = chunk.Sections[y];
            if (section != _emptySection && create)
                return section;

            section = CreateEmptySection();
            chunk.Sections[y] = section;
            return section;
        }

        public void SetSkyLight(Chunk chunk, int x, int y, int z, byte light)
        {
            GetBlock(chunk, x, y, z, true).SkyLight = light;
        }

        private ChunkSection CreateEmptySection()
        {
            ChunkSection section = new ChunkSection();
            section.Blocks = new Block[ChunkSection.Width, ChunkSection.Height, ChunkSection.Width];

            for (int x = 0; x < ChunkSection.Width; x++)
            {
                for (int y = 0; y < ChunkSection.Width; y++)
                {
                    for (int z = 0; z < ChunkSection.Width; z++)
                    {
                        BlockType airBlockType = _blockTypeService.GetAirBlock();
                        section.Blocks[x, y, z] = new Block(airBlockType, 0, 0);
                    }
                }
            }

            return section;
        }

    }
}
