using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.ItemStack
{
    public interface IItemStack
    {
        string Name { get; }
        int Size { get; }
        int MaximumSize { get; }
        int Durability { get; }
        int MaxDurability { get; }

        IItemStack ChangeSize(int count);
        IItemStack ChangeDurability(int damage);
    }
}
