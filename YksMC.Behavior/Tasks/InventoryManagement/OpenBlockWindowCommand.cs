using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Chunk;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class OpenBlockWindowCommand
    {
        public IBlockLocation Location { get; set; }

        public OpenBlockWindowCommand(IBlockLocation location)
        {
            Location = location;
        }
    }
}
