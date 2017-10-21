using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;

namespace YksMC.Behavior.Tasks
{
    public class HarvestBlockCommand
    {
        public IBlockLocation Location { get; private set; }
        public bool FailWhenNoRightToolAvailable { get; set; }

        public HarvestBlockCommand(IBlockLocation location, bool failWhenNoRightToolAvailable = false)
        {
            Location = location;
            FailWhenNoRightToolAvailable = failWhenNoRightToolAvailable;
        }
    }
}
