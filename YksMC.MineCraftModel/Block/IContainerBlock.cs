using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Inventory;
using YksMC.MinecraftModel.ItemStack;

namespace YksMC.MinecraftModel.Block
{
    public interface IContainerBlock : IBlock
    {
        IInventory GetInventory();
        IContainerBlock WithInventorySlot(int slot, IItemStack itemStack);
    }
}
