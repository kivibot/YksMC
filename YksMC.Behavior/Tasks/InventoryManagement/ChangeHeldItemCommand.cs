using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class ChangeHeldItemCommand
    {
        public short HotbarSlot { get; private set; }

        public ChangeHeldItemCommand(int hotbarSlot)
        {
            if(hotbarSlot < 0 || hotbarSlot >= 9)
            {
                throw new ArgumentException(nameof(hotbarSlot));
            }
            HotbarSlot = (short)hotbarSlot;
        }
    }
}
