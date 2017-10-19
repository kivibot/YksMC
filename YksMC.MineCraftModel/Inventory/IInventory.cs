using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.ItemStack;

namespace YksMC.MinecraftModel.Inventory
{
    public interface IInventory
    {
        int SlotCount { get; }

        IInventory ChangeSlot(int slotInde, IItemStack itemStack);
        TSlot GetSlot<TSlot>(int slotInde) where TSlot : class, IItemStack;
    }
}
