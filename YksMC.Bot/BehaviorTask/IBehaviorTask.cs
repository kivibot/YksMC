using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public interface IBehaviorTask
    {
        string Name { get; }
        BehaviorTaskPriority Priority { get; }

        bool IsPossible(IWorld world);

        IBehaviorTaskEventResult OnStart(IWorld world);
        Task<bool?> OnStartAsync(IWorld world);

        IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick);
    }
}
