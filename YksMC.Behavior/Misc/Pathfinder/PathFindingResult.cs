using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.Behavior.Misc.Pathfinder
{
    internal class PathFindingResult : IPathFindingResult
    {
        private readonly bool _failed;
        private readonly IReadOnlyList<IPathWaypoint> _path;

        public bool Failed => _failed;
        public IReadOnlyList<IPathWaypoint> Path => _path;

        public PathFindingResult()
        {
            _failed = true;
        }

        public PathFindingResult(IReadOnlyList<IPathWaypoint> path)
        {
            _failed = path.Count == 0;
            _path = path;
        }
    }
}
