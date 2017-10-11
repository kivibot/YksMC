using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;

namespace YksMC.MinecraftModel.Dimension
{
    public static class DimensionExtensions
    {
        public static IPlayer GetLocalPlayer(this IDimension dimension)
        {
            return dimension.GetPlayers().FirstOrDefault();
        }


        public static IEntity GetLocalPlayerEntity(this IDimension dimension)
        {
            return dimension.GetEntity(dimension.GetLocalPlayer().EntityId);
        }

    }
}
