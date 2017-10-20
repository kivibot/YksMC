using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.ItemStack;

namespace YksMC.MinecraftModel.Inventory
{
    public interface IInventory
    {
        int SlotCount { get; }

        IInventory ChangeSlot(int slotIndex, IItemStack itemStack);
        TSlot GetSlot<TSlot>(int slotIndex) where TSlot : class, IItemStack;
    }
}
