using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;

namespace YksMC.MinecraftModel.Dimension
{
    public static class DimensionExtensions
    {
        public static IDimension ChangeAgeAndTime(this IDimension dimension, Func<IAgeTime, IAgeTime> getAgeAndTime)
        {
            return dimension.ChangeAgeAndTime(getAgeAndTime(dimension.AgeAndTime));
        }

        public static T GetBlock<T>(this IDimension dimension, IBlockLocation blockLocation) where T : class, IBlock
        {
            IChunkCoordinate chunkLocation = new ChunkCoordinate(blockLocation);
            IChunk chunk = dimension.GetChunk(chunkLocation);
            return chunk.GetBlock<T>(blockLocation);
        }
    }
}
