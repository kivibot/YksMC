using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.WorldEvent
{
    public interface IWorldEvent<T>
    {
        IWorld World { get; }
        T Event { get; }
    }
}
