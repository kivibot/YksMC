using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Misc
{
    public interface IPlayerCollisionDetectionService
    {
        IWorld UpdatePlayerPosition(IWorld world, IVector3<double> movement);
    }
}
