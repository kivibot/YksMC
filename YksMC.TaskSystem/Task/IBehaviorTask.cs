using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Task
{
    public interface IBehaviorTask
    {
        string Name { get; }

        bool IsCompleted { get; }
        bool IsFailed { get; }

        IWorld OnStart(IWorld world);
        IWorld OnTick(IWorld world);
        void OnPacketReceived(object packet);
    }
}
