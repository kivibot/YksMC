using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Chunk;

namespace YksMC.Behavior.Misc.Pathfinder
{
    internal class PathWaypoint : IPathWaypoint
    {
        private readonly IBlockLocation _target;
        private readonly PathMovementType _movementType;

        public IBlockLocation Target => _target;
        public PathMovementType MovementType => _movementType;

        public PathWaypoint(IBlockLocation target, PathMovementType movementType)
        {
            _target = target;
            _movementType = movementType;
        }
    }
}
