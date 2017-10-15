using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.WorldEvent
{
    public abstract class WorldEventHandler
    {
        protected IWorldEventResult Result(IWorld world, params object[] replyPackets)
        {
            return new WorldEventResult(world, replyPackets.ToList());
        }
    }
}
