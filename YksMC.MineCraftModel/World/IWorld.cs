using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;

namespace YksMC.MinecraftModel.World
{
    public interface IWorld
    {
        IDimension Dimension { get; }

        IChunk GetChunk(IChunkCoordinate position);
        IWorld ChangeChunk(IChunkCoordinate position, IChunk chunk);

        IEntity GetEntity(int id);
        IWorld ChangeEntity(IEntity entity);

        IWorld ReplacePlayer(IPlayer player);
        IEnumerable<IPlayer> GetPlayers();
    }
}
