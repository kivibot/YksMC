using System;
using System.Collections.Generic;
using System.Text;
using YksMC.EventBus.Bus;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.WorldEvent
{
    public interface IWorldEventHandler<T> : IEventHandler<IWorldEvent<T>, IWorldEventResult>
    {
    }
}
