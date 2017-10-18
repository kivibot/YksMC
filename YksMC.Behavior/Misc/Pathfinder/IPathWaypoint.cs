﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.Behavior.Misc.Pathfinder
{
    public interface IPathWaypoint
    {
        IBlockLocation Target { get; }
        PathMovementType MovementType { get; }
    }
}
