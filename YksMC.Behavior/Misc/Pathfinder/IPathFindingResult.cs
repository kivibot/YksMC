using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.Behavior.Misc.Pathfinder
{
    public interface IPathFindingResult
    {
        bool Failed { get; }
        IReadOnlyList<IPathWaypoint> Path { get; }
    }
}
