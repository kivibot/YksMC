using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Dimension;

namespace YksMC.MinecraftModel.World
{
    public static class WorldExtensions
    {
        public static IWorld ChangeCurrentDimension(this IWorld world, Func<IDimension, IDimension> getDimension)
        {
            return world.ReplaceCurrentDimension(getDimension(world.GetCurrentDimension()));
        }
    }
}
