using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.Behavior.Misc
{
    public class PathFindingResult : IPathFindingResult
    {
        private readonly bool _failed;
        private readonly IReadOnlyList<IBlockLocation> _path;

        public bool Failed => _failed;
        public IReadOnlyList<IBlockLocation> Path => _path;

        public PathFindingResult()
        {
            _failed = true;
        }

        public PathFindingResult(IReadOnlyList<IBlockLocation> path)
        {
            _path = path;
        }
    }
}
