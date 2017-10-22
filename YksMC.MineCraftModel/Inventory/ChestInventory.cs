using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.ItemStack;

namespace YksMC.MinecraftModel.Inventory
{
    public class ChestInventory : Inventory, IChestInventory
    {
        public ChestInventory() 
            : base(27)
        {
        }

        protected ChestInventory(IItemStack[] slots) 
            : base(slots)
        {
        }

        protected override IInventory CreateInventory(IItemStack[] slots)
        {
            return new ChestInventory(slots);
        }
    }
}
