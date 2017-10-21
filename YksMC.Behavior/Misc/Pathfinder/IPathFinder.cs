using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Dimension;

namespace YksMC.Behavior.Misc.Pathfinder
{
    public interface IPathFinder
    {
        IPathFindingResult FindPath(IBlockLocation startLocation, IBlockLocation endLocation, IDimension dimension);
    }
}
