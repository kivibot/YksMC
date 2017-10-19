using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Entity;

namespace YksMC.Behavior.Tasks
{
    public class LookAtCommand
    {
        public IEntityLocation Location { get; private set; }

        public LookAtCommand(IEntityLocation location)
        {
            Location = location;
        }
    }
}
