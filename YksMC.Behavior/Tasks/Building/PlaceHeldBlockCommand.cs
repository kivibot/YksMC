using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;

namespace YksMC.Behavior.Tasks.Building
{
    public class PlaceHeldBlockCommand
    {
        public IBlockLocation Location { get; set; }

        public PlaceHeldBlockCommand(IBlockLocation location)
        {
            Location = location;
        }
    }
}
