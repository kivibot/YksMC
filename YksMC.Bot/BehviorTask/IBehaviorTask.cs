using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public interface IBehaviorTask
    {
        string Name { get; }

        bool IsCompleted { get; }
        bool IsFailed { get; }

        IWorld OnStart(IWorld world);
        void OnTick(IWorld world);
    }
}
