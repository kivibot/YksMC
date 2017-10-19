using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.ItemStack;

namespace YksMC.MinecraftModel.Inventory
{
    public class Inventory : IInventory
    {
        protected readonly IItemStack[] _slots;

        public int SlotCount => _slots.Length;

        public Inventory(int slotCount)
        {
            _slots = new IItemStack[slotCount];
        }

        protected Inventory(IItemStack[] slots)
        {
            _slots = slots;
        }

        public virtual IInventory ChangeSlot(int slotIndex, IItemStack itemStack)
        {
            if (slotIndex < 0 || slotIndex > _slots.Length)
            {
                throw new ArgumentException(nameof(slotIndex));
            }
            IItemStack[] slots = (IItemStack[])_slots.Clone();
            slots[slotIndex] = itemStack;
            return CreateInventory(slots);
        }

        public virtual TSlot GetSlot<TSlot>(int slotIndex) where TSlot : class, IItemStack
        {
            if (slotIndex < 0 || slotIndex > _slots.Length)
            {
                throw new ArgumentException(nameof(slotIndex));
            }
            return _slots[slotIndex] as TSlot;
        }

        protected virtual IInventory CreateInventory(IItemStack[] slots)
        {
            return new Inventory(slots);
        }
    }
}
