using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class PickUpWindowSlotCommand
    {
        public int WindowId { get; set; }
        public int Slot { get; set; }

        public PickUpWindowSlotCommand(int windowId, int slot)
        {
            WindowId = windowId;
            Slot = slot;
        }
    }
}
