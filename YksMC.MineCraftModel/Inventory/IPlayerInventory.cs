using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.ItemStack;

namespace YksMC.MinecraftModel.Inventory
{
    public interface IPlayerInventory : IInventory
    {
        TSlot GetCraftingSlot<TSlot>(int x, int y) where TSlot : class, IItemStack;
        TSlot GetOutputSlot<TSlot>() where TSlot : class, IItemStack;

        TSlot GetMainInventorySlot<TSlot>(int slotIndex) where TSlot : class, IItemStack;
        TSlot GetHotbarSlot<TSlot>(int slotIndex) where TSlot : class, IItemStack;
    }
}
