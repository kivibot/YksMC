using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public abstract class BehaviorTask<T> : WorldEventHandler, IBehaviorTask
    {
        private bool _isCompleted;
        private bool _isFailed;

        protected T _command;

        public abstract string Name { get; }
        public bool IsCompleted => _isCompleted;
        public bool IsFailed => _isFailed;

        public BehaviorTask(T command)
        {
            _command = command;
        }

        public abstract IWorldEventResult OnStart(IWorld world);

        public abstract void OnTick(IWorld world, IGameTick tick);

        protected void Fail()
        {
            _isFailed = true;
            _isCompleted = true;
        }

        protected void Complete()
        {
            _isCompleted = true;
        }
    }
}
