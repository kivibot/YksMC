using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Chunk;
using YksMC.Abstraction.World;

namespace YksMC.Bot.Chunk
{
    public class ChunkService : IChunkService
    {
        private const int _chunkWidth = 16;
        private const int _chunkHeight = 32 * 16;
        private const int _chunkArea = _chunkWidth * _chunkWidth;
        private const int _chunkVolume = _chunkHeight * _chunkArea;

        private readonly int _diameter;
        private readonly Chunk[] _chunks;

        public ChunkService(ChunkServiceOptions options)
        {
            _diameter = options.Diameter;
            _chunks = new Chunk[_diameter * _diameter];
        }

        public IChunk CreateChunk(Dimension dimension, int x, int z)
        {
            int chunkIndex = GetChunkIndex(x, z);
            if (_chunks[chunkIndex] != null)
            {
                throw new ArgumentException($"Chunk already exists! [{x},{z}] ({chunkIndex})");
            }
            Chunk chunk = new Chunk()
            {
                Dimension = dimension,
                X = x,
                Z = z,
                Biomes = new Biome[_chunkArea],
                BlockTypes = new IBlockType[_chunkVolume],
                BlockLightLevels = new byte[_chunkVolume],
                SkyLightLevels = new byte[_chunkVolume]
            };
            _chunks[chunkIndex] = chunk;
            return chunk;
        }

        public IChunk GetChunk(Dimension dimension, int x, int z)
        {
            int chunkIndex = GetChunkIndex(x, z);
            Chunk chunk = _chunks[chunkIndex];
            if (_chunks[chunkIndex] == null)
            {
                return null;
            }
            return chunk;
        }

        public IBlock GetBlock(BlockLocation location)
        {
            if (!TryGetChunk(location, out Chunk chunk))
            {
                return null;
            }

            int index = GetBlockIndex(location);

            return new Block()
            {
                Type = chunk.BlockTypes[index],
                BlockLight = chunk.BlockLightLevels[index],
                SkyLight = chunk.SkyLightLevels[index],
                Biome = chunk.Biomes[GetBlockIndex(location)]
            };
        }

        public void SetBiome(Dimension dimension, int x, int z, Biome biome)
        {
            BlockLocation location = new BlockLocation(dimension, x, 0, z);
            if (!TryGetChunk(location, out Chunk chunk))
            {
                throw new ArgumentException("Unloaded chunk.");
            }
            chunk.Biomes[GetBiomeIndex(location)] = biome;
        }

        public void SetBlockLight(BlockLocation location, byte lightLevel)
        {
            if (!TryGetChunk(location, out Chunk chunk))
            {
                throw new ArgumentException("Unloaded chunk.");
            }
            int index = GetBlockIndex(location);
            chunk.BlockLightLevels[index] = lightLevel;
        }

        public void SetBlockType(BlockLocation location, IBlockType type)
        {
            if (!TryGetChunk(location, out Chunk chunk))
            {
                throw new ArgumentException("Unloaded chunk.");
            }
            int index = GetBlockIndex(location);
            chunk.BlockTypes[index] = type;
        }

        public void SetSkyLight(BlockLocation location, byte lightLevel)
        {
            if (!TryGetChunk(location, out Chunk chunk))
            {
                throw new ArgumentException("Unloaded chunk.");
            }
            int index = GetBlockIndex(location);
            chunk.SkyLightLevels[index] = lightLevel;
        }

        private bool TryGetChunk(BlockLocation location, out Chunk chunk)
        {
            int x = (int)Math.Floor(location.X / (float)_chunkWidth);
            int z = (int)Math.Floor(location.Z / (float)_chunkWidth);
            int index = GetChunkIndex(x, z);
            chunk = _chunks[index];
            if (chunk == null)
            {
                return false;
            }
            return true;
        }

        private int GetBlockIndex(BlockLocation location)
        {
            int x = Modulo(location.X, _chunkWidth);
            int y = location.Y;
            int z = Modulo(location.Z, _chunkWidth); ;

            return _chunkArea * y + _chunkWidth * z + x;
        }

        private int GetBiomeIndex(BlockLocation location)
        {
            int x = Modulo(location.X, _chunkWidth);
            int z = Modulo(location.Z, _chunkWidth);

            return _chunkWidth * x + z;
        }

        private int GetChunkIndex(int x, int z)
        {
            return Modulo(x, _diameter) * _diameter + Modulo(z, _diameter);
        }

        private int Modulo(int a, int b)
        {
            int remainder = a % b;
            if (remainder < 0)
            {
                return b + remainder;
            }
            return remainder;
        }
    }
}
