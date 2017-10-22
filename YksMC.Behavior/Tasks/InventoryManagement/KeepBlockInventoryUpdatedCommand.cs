using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Chunk;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class KeepBlockInventoryUpdatedCommand
    {
        public IBlockLocation Location { get; set; }
        public int WindowId { get; set; }

        public KeepBlockInventoryUpdatedCommand(IBlockLocation location, int windowId)
        {
            Location = location;
            WindowId = windowId;
        }
    }
}
