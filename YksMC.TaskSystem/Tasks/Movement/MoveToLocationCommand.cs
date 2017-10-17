using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Entity;

namespace YksMC.Behavior.Tasks.Movement
{
    public class MoveToLocationCommand
    {
        public IEntityLocation Location { get; private set; }
        public double MinimumDistance { get; private set; }

        public MoveToLocationCommand(IEntityLocation location, double minimumDistance = 0.1)
        {
            Location = location;
            MinimumDistance = minimumDistance;
            if(minimumDistance < 0.01)
            {
                throw new ArgumentException("Minimum distance too low!");
            }
        }
    }
}
