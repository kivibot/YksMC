using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;

namespace YksMC.MinecraftModel.Dimension
{
    public interface IDimension
    {
        int Id { get; }
        IDimensionType Type { get; }

        IChunk GetChunk(IChunkCoordinate position);
        IDimension ChangeChunk(IChunkCoordinate position, IChunk chunk);

        IEntity GetEntity(int id);
        IDimension ChangeEntity(IEntity entity);
    }
}
