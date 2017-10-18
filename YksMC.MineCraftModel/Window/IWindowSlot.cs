using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.ItemType;

namespace YksMC.MinecraftModel.Window
{
    public interface IWindowSlot
    {
        IItemType ItemType { get; }
        int ItemCount { get; }
        int ItemDamage { get; }
    }
}
