using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;

namespace YksMC.MinecraftModel.World
{
    public static class WorldExtensions
    {
        public static IPlayer GetLocalPlayer(this IWorld world)
        {
            return world.GetPlayers().FirstOrDefault();
        }


        public static IEntity GetLocalPlayerEntity(this IWorld world)
        {
            return world.GetEntity(world.GetLocalPlayer().EntityId);
        }

    }
}
