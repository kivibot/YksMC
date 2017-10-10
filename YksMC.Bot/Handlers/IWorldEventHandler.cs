using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.Handlers
{
    public interface IWorldEventHandler<T>
    {
        IWorld ApplyEvent(T eventArgs, IWorld world);
    }
}
