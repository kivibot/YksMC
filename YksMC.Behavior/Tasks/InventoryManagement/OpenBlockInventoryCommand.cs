using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Chunk;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class OpenBlockInventoryCommand
    {
        public IBlockLocation Location { get; set; }

        public OpenBlockInventoryCommand(IBlockLocation location)
        {
            Location = location;
        }
    }
}
