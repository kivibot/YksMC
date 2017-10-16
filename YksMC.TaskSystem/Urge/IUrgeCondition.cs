﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Urge
{
    public interface IUrgeCondition
    {
        string Name { get; }

        bool IsPossible(IWorld world);
    }
}
