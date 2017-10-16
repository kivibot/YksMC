using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Behavior.Task
{
    public interface IBehaviorTaskManager
    {
        IBehaviorTask GetTask(string name);
    }
}
