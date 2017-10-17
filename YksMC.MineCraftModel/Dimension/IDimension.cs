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
        IAgeTime AgeAndTime { get; }
        IEntityCollection Entities { get; }

        IChunk GetChunk(IChunkCoordinate position);
        IDimension ReplaceChunk(IChunkCoordinate position, IChunk chunk);        
        IDimension ReplaceEntity(IEntity entity);
        IDimension ChangeAgeAndTime(IAgeTime ageAndTime);
    }
}
