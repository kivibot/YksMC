using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;

namespace YksMC.MinecraftModel.World
{
    public static class WorldExtensions
    {
        public static IWorld ChangeCurrentDimension(this IWorld world, Func<IDimension, IDimension> getDimension)
        {
            return world.ReplaceCurrentDimension(getDimension(world.GetCurrentDimension()));
        }

        public static IEntity GetPlayerEntity(this IWorld world)
        {
            IPlayer player = world.GetLocalPlayer();
            return world.GetDimension(player.DimensionId).Entities[player.EntityId];
        }
    }
}
