using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Entity;

namespace YksMC.Behavior.Tasks.Movement
{
    public class WalkToLocationCommand
    {
        public IBlockLocation Location { get; private set; }

        public WalkToLocationCommand(IBlockLocation location)
        {
            Location = location;
        }
    }
}
