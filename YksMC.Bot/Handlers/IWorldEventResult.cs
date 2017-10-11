using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.Handlers
{
    public interface IWorldEventResult
    {
        IWorld World { get; }
        IReadOnlyList<object> ServerBoundPackets { get; }
    }
}
