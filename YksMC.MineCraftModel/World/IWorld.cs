using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;

namespace YksMC.MinecraftModel.World
{
    public interface IWorld
    {
        IDimension Dimension { get; }

        IChunk GetChunk(IChunkCoordinate position);
        IWorld ChangeChunk(IChunkCoordinate position, IChunk chunk);
    }
}
