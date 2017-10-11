using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Dimension;

namespace YksMC.Bot.Handlers
{
    public interface IWorldEventHandler<T>
    {
        IDimension ApplyEvent(T eventArgs, IDimension dimension);
    }
}
