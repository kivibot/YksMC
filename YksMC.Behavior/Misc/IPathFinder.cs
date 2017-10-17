using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;

namespace YksMC.Behavior.Misc
{
    public interface IPathFinder
    {
        IPathFindingResult FindPath(IBlockLocation startLocation, IBlockLocation endLocation, IDimension dimension);
    }
}
