using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.TickHandlers
{
    public class PlayerMovementHandler : WorldEventHandler, IWorldEventHandler<IGameTick>
    {
        private const double _acceleration = 0.08;
        private const double _drag = 0.02;
        private const double _maxVelocity = 3.92;

        public IWorldEventResult ApplyEvent(IGameTick tick, IWorld world)
        {
            IPlayer player = world.GetLocalPlayer();
            if (!player.HasEntity)
            {
                return Result(world);
            }
            IEntity entity = world.GetCurrentDimension().GetEntity(player.EntityId);
            IBlockCoordinate blockLocation = new BlockCoordinate((int)entity.Location.X, (int)entity.Location.Y - 1, (int)entity.Location.Z);
            if (blockLocation.Y < 0)
            {
                return Result(world);
            }
            IChunkCoordinate chunkLocation = new ChunkCoordinate(blockLocation);
            IBlock block = world.GetCurrentDimension().GetChunk(chunkLocation).GetBlock(blockLocation);
            if (block.Type.Name != "air" && entity.Location.Y == blockLocation.Y + 1)
            {
                return Result(world);
            }
            IEntityLocation location = new EntityLocation(entity.Location.X, entity.Location.Y - 0.07, entity.Location.Z);

            return Result(world.ReplaceCurrentDimension(world.GetCurrentDimension().ChangeEntity(entity.ChangeLocation(location))));
        }
    }
}
