using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Dimension;

namespace YksMC.Bot.Handlers
{
    public interface IWorldEventResult
    {
        IDimension World { get; }
        IReadOnlyList<object> ServerBoundPackets { get; }
    }
}
