using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.WorldEvent
{
    public interface IWorldEventResult
    {
        IWorld World { get; }
        IReadOnlyList<object> ReplyPackets { get; }
    }
}
