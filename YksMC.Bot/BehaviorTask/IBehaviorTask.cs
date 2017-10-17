using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public interface IBehaviorTask
    {
        string Name { get; }

        bool IsCompleted { get; }
        bool IsFailed { get; }

        IWorldEventResult OnStart(IWorld world);
        void OnTick(IWorld world, IGameTick tick);
    }
}
