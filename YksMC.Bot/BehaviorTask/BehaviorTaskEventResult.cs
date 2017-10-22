using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public class BehaviorTaskEventResult : IBehaviorTaskEventResult
    {
        private readonly IWorld _world;
        private readonly bool _isFailed;
        private readonly bool _isCompleted;

        public IWorld World => _world;
        public bool IsFailed => _isFailed;
        public bool IsCompleted => _isCompleted;

        public BehaviorTaskEventResult(IWorld world, bool isFailed, bool isCompleted)
        {
            _world = world;
            _isFailed = isFailed;
            _isCompleted = isCompleted;
        }
    }
}
