using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class SwapWindowSlotsCommand
    {
        public int WindowId { get; set; }
        public int SourceSlot { get; set; }
        public int TargetSlot { get; set; }

        public SwapWindowSlotsCommand(int windowId, int sourceSlot, int targetSlot)
        {
            WindowId = windowId;
            SourceSlot = sourceSlot;
            TargetSlot = targetSlot;
        }
    }
}
