using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.Behavior.Tasks
{
    public class HarvestBlockCommand
    {
        public IBlockLocation Location { get; private set; }

        public HarvestBlockCommand(IBlockLocation location)
        {
            Location = location;
        }
    }
}
