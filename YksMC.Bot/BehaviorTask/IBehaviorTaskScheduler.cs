using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public interface IBehaviorTaskScheduler
    {
        void EnqueueCommand(object command);
        Task<bool> RunCommandAsync(object command);

        IWorld HandleTick(IWorld world, IGameTick tick);
    }
}
