using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.BehaviorTask
{
    public interface IBehaviorTaskManager
    {
        IBehaviorTask GetTask(object command);
    }
}
