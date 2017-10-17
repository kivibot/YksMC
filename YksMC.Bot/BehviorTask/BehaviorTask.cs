using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public abstract class BehaviorTask : IBehaviorTask
    {
        private readonly string _name;
        private bool _isCompleted;
        private bool _isFailed;

        public string Name => _name;
        public bool IsCompleted => _isCompleted;
        public bool IsFailed => _isFailed;

        public BehaviorTask(string name)
        {
            _name = name;
        }

        public abstract IWorld OnStart(IWorld world);

        public abstract void OnTick(IWorld world);

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
