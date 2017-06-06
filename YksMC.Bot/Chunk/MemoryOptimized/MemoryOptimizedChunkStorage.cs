using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;

namespace YksMC.Bot.Chunk.MemoryOptimized
{
    public class MemoryOptimizedChunkStorage : IChunkStorage
    {
        private readonly Dictionary<Tuple<int, int>, DataChunk> _chunks = new Dictionary<Tuple<int, int>, DataChunk>();

        public IChunk CreateChunk(Dimension dimension, int x, int z)
        {
            //TODO: check that the chunk does not exist
            DataChunk chunk = new DataChunk(dimension, x, z);
            chunk.Blocks = new DataBlock[16, 256, 16]; //TODO: use named constants
            _chunks[new Tuple<int, int>(x, z)] = chunk;
            return chunk;
        }

        public IBlock GetBlock(BlockLocation location)
        {
            if (TryGetDataBlock(location, out DataBlock block))
            {
                byte blockLight = (byte)(block.LightData & 0b1111);
                byte skyLight = (byte)(block.LightData >> 4);
                return new Block(null, blockLight, skyLight); //TODO: implement block type
            }
            //TODO: implement
            throw new NotImplementedException();
        }


        public IChunk GetChunk(Dimension dimension, int x, int z)
        {
            //TODO: implement
            throw new NotImplementedException();
        }

        public void SetBlockLight(BlockLocation location, byte level)
        {
            if (TryGetDataBlock(location, out DataBlock block))
            {
                block.LightData = (byte)((0b11110000 & block.LightData) | level);
            }
        }

        public void SetBlockType(BlockLocation location, IBlockType type)
        {
            if (TryGetDataBlock(location, out DataBlock block))
            {
                block.BlockId = (byte)type.Id;
                block.BlockMeta = (byte)type.Data;
            }
        }

        public void SetSkyLight(BlockLocation location, byte level)
        {
            if (TryGetDataBlock(location, out DataBlock block))
            {
                block.LightData = (byte)((0b00001111 & block.LightData) | (level << 4));
            }
        }

        private bool TryGetDataBlock(BlockLocation location, out DataBlock block)
        {
            if (!TryGetDataChunk(location.Dimension, location.X / 16, location.Z / 16, out DataChunk chunk))
            {
                block = default(DataBlock);
                return false;
            }
            block = chunk.Blocks[Modulo(location.X, 16), location.Y, Modulo(location.Z, 16)];
            return true;
        }

        private int Modulo(int a, int b)
        {
            if (a < 0)
            {
                return b - ((-a) % b) - 1;
            }
            return a % b;
        }

        private bool TryGetDataChunk(Dimension dimension, int x, int z, out DataChunk chunk)
        {
            return _chunks.TryGetValue(new Tuple<int, int>(x, z), out chunk);
        }
    }
}
