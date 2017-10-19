using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YksMC.MinecraftModel.Window
{
    public class Window : IWindow
    {
        private readonly int _id;
        private readonly string _title;
        private readonly IReadOnlyList<IWindowSlot> _slots;

        public int Id => _id;
        public string Title => _title;
        public IReadOnlyList<IWindowSlot> Slots => _slots;

        public Window(int id, string title, IReadOnlyList<IWindowSlot> slots)
        {
            _id = id;
            _title = title;
            _slots = slots;
        }

        public IWindow ReplaceSlot(IWindowSlot slot)
        {
            List<IWindowSlot> slots = _slots.ToList();
            slots.Add(slot);
            return new Window(_id, _title, slots);
        }
    }
}
