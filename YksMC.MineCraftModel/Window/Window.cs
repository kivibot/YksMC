using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.ItemStack;

namespace YksMC.MinecraftModel.Window
{
    public class Window : IWindow
    {
        private const int _playerInventorySize = 4 * 9;

        private readonly int _id;
        private readonly string _name;
        private readonly string _title;
        private readonly IItemStack[] _slots;

        public int Id => _id;
        public string Title => _title;
        public IItemStack[] Slots => _slots;
        public string Name => _name;

        public Window(string name)
        {
            _name = name;
            _slots = new IItemStack[0];
        }

        protected Window(int id, string name, string title, IItemStack[] slots)
        {
            _id = id;
            _name = name;
            _title = title;
            _slots = slots;
        }

        public IWindow WithSlot(int slotIndex, IItemStack slot)
        {
            IItemStack[] slots = (IItemStack[])_slots.Clone();
            slots[slotIndex] = slot;
            return new Window(_id, _name, _title, slots);
        }

        public IWindow WithId(int id)
        {
            return new Window(id, _name, _title, _slots);
        }

        public IWindow WithTitle(string title)
        {
            return new Window(_id, _name, title, _slots);
        }

        public IWindow WithUniqueSlots(int slotCount)
        {
            IItemStack[] slots = new IItemStack[slotCount + _playerInventorySize];
            Array.Copy(_slots, slots, Math.Min(_slots.Length, slotCount + _playerInventorySize));
            return new Window(_id, _name, _title, slots);
        }
    }
}
