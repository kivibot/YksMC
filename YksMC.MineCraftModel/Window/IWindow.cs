using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.ItemStack;

namespace YksMC.MinecraftModel.Window
{
    public interface IWindow
    {
        int Id { get; }
        string Name { get; }
        string Title { get; }
        IItemStack[] Slots { get; }

        IWindow WithId(int id);
        IWindow WithUniqueSlots(int slotCount);
        IWindow WithTitle(string title);
        IWindow WithSlot(int slotIndex, IItemStack slot);
    }
}
