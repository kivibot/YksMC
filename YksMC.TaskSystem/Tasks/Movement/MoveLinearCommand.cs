using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Entity;

namespace YksMC.Behavior.Tasks.Movement
{
    public class MoveLinearCommand
    {
        public IVector3<double> Movement { get; private set; }

        public MoveLinearCommand(IVector3<double> movement)
        {
            Movement = movement;
        }
    }
}
