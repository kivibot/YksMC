using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.ItemStack;

namespace YksMC.MinecraftModel.Inventory
{
    public class PlayerInventory : Inventory, IPlayerInventory
    {
        private const int _slotCount = 46;
        private const int _hotbarOffset = 36;
        private const int _hotbarSize = 9;
        private const int _outputOffset = 0;
        private const int _mainInventoryOffset = 9;
        private const int _mainInventorySize = 27;
        private const int _craftingGridOffset = 1;
        private const int _craftingWidth = 2;

        public PlayerInventory()
            : base(_slotCount)
        {
        }

        protected PlayerInventory(IItemStack[] slots)
            : base(slots)
        {

        }

        public TSlot GetCraftingSlot<TSlot>(int x, int y) where TSlot : class, IItemStack
        {
            if (x < 0 || x >= _craftingWidth)
            {
                throw new ArgumentException(nameof(x));
            }
            if (y < 0 || y >= _craftingWidth)
            {
                throw new ArgumentException(nameof(y));
            }
            int slotIndex = _craftingGridOffset + x + 2 * y;
            return GetSlot<TSlot>(slotIndex);
        }

        public TSlot GetHotbarSlot<TSlot>(int slotIndex) where TSlot : class, IItemStack
        {
            if (slotIndex < 0 || slotIndex >= _hotbarSize)
            {
                throw new ArgumentException(nameof(slotIndex));
            }
            return GetSlot<TSlot>(_hotbarOffset + slotIndex);
        }

        public TSlot GetMainInventorySlot<TSlot>(int slotIndex) where TSlot : class, IItemStack
        {
            if (slotIndex < 0 || slotIndex >= _mainInventorySize)
            {
                throw new ArgumentException(nameof(slotIndex));
            }
            return GetSlot<TSlot>(_hotbarOffset + slotIndex);
        }

        public TSlot GetOutputSlot<TSlot>() where TSlot : class, IItemStack
        {
            return GetSlot<TSlot>(_outputOffset);
        }

        protected override IInventory CreateInventory(IItemStack[] slots)
        {
            return base.CreateInventory(slots);
        }
    }
}
