using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public abstract class BehaviorTask<T> : IBehaviorTask
    {
        protected readonly T _command;
        protected readonly IMinecraftClient _minecraftClient;
        protected readonly IBehaviorTaskScheduler _taskScheduler;

        private BehaviorTaskPriority _priority;

        public abstract string Name { get; }
        public BehaviorTaskPriority Priority => _priority;

        public BehaviorTask(T command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler)
        {
            _command = command;
            _minecraftClient = minecraftClient;
            _taskScheduler = taskScheduler;
            _priority = BehaviorTaskPriority.Normal;
        }

        protected void ChangePriority(BehaviorTaskPriority priority)
        {
            _priority = priority;
        }

        protected IBehaviorTaskEventResult Failure(IWorld world)
        {
            return new BehaviorTaskEventResult(world, true, true);
        }

        protected IBehaviorTaskEventResult Success(IWorld world)
        {
            return new BehaviorTaskEventResult(world, false, true);
        }

        protected IBehaviorTaskEventResult Result(IWorld world)
        {
            return new BehaviorTaskEventResult(world, false, false);
        }

        public abstract bool IsPossible(IWorld world);

        public abstract IBehaviorTaskEventResult OnStart(IWorld world);

        public virtual Task<bool?> OnStartAsync(IWorld world) => Task.FromResult<bool?>(null);

        public abstract IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick);
      
    }
}
