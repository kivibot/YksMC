using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.Behavior.Misc.Pathfinder
{
    internal class PathEdge
    {
        private readonly IBlockLocation _from;
        private readonly IBlockLocation _to;
        private readonly PathMovementType _movementType;
        private readonly double _cost;

        public IBlockLocation From => _from;
        public IBlockLocation To => _to;
        public PathMovementType MovementType => _movementType;
        public double CumulativeCost => _cost;

        public PathEdge(IBlockLocation from, IBlockLocation to, PathMovementType movementType, double cost)
        {
            _from = from;
            _to = to;
            _movementType = movementType;
            _cost = cost;
        }
    }
}
