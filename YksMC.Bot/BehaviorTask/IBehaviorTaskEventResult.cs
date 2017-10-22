using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public interface IBehaviorTaskEventResult
    {
        bool IsFailed { get; }
        bool IsCompleted { get; }
        IWorld World { get; }
    }
}
