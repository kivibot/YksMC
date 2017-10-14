using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.WorldEvent
{
    public interface IWorldEventHandler<T>
    {
        IWorldEventResult ApplyEvent(T eventArgs, IWorld world);
    }
}
