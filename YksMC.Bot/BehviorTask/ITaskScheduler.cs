using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.BehaviorTask
{
    public interface ITaskScheduler
    {
        void EnqueueTask(IBehaviorTask task);
    }
}
