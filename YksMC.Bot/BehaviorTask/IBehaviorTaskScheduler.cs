﻿using System;
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
        void EnqueueTask(IBehaviorTask task);
        IWorldEventResult HandleTick(IWorld world, IGameTick tick);
        Task<IBehaviorTask> RunTaskAsync(IBehaviorTask task);
    }
}
