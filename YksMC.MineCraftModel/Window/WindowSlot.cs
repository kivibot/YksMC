using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.ItemType;

namespace YksMC.MinecraftModel.Window
{
    public class WindowSlot : IWindowSlot
    {
        private readonly IItemType _itemType;
        private readonly int _itemCount;
        private readonly int _itemDamage;

        public IItemType ItemType => _itemType;
        public int ItemCount => _itemCount;
        public int ItemDamage => _itemDamage;

        public WindowSlot(IItemType  itemType, int itemCount, int itemDamage)
        {
            _itemType = itemType;
            _itemCount = itemCount;
            _itemDamage = itemDamage;
        }
    }
}
